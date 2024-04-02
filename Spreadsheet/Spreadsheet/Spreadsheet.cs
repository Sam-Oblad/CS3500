/// <summary>
/// Author: Sam Oblad
/// Partner: None
/// Date: 2/10/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// I, Sam Oblad, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
/// 
/// File Contents
/// This file contains my Spreadsheet class implementation. It inherits from AbstractSpreadsheet. I added two additional helper methods that
/// validate names and extract variables/remove dependencies. All public methods inherited from AbstractSpreadsheet also inherit documentation by use of the
/// <inheritdoc/> XML tag. 

/// </summary>
using SpreadsheetUtilities;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
namespace SS
{
    /// <summary>
    /// This is my spreadsheet class, it inherits from the given abstract class: AbstractSpreadsheet
    /// My implementation uses a dictionary to store names to cell contents. I also use a second dictionary to store names to cell values. These values
    /// are calculated upon a new cell being populated with contents. If a cell contains a formula with another cell, and a cell within this dependency changes, 
    /// my spreadsheet will recalculate the values in a specific order given by getCellsToRecalculate and then restore them in the values dictionary.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> spreadsheet;
        private Dictionary<string, object> values;
        private DependencyGraph dependencyGraph;
        private string? FilePath;
        public override bool Changed { get; protected set; }

        /// <summary>
        /// This is the default constructor for my spreadsheet class, it has no parameters. Note that its IsValid function is just our default validator, our normalizer normalizes
        /// to itself, and our version is set to default
        /// </summary>
        public Spreadsheet() : base(IsValidName, s => s, "default")
        {
            spreadsheet = new();
            dependencyGraph = new();
            values = new();
            Changed = false;
        }

        /// <summary>
        /// This is my three parameter constructor for a spreadsheet. It takes two delegate, isValid, and normalize, as well as a string for the version
        /// of spreadsheet
        /// </summary>
        /// <param name="isValid">Additional validator user provided</param>
        /// <param name="normalize">Normalizer of names user provided</param>
        /// <param name="version">this represents the version of spreadsheet, and is the first line in a saved file in xml</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            spreadsheet = new();
            dependencyGraph = new();
            values = new();
            Changed = false;
        }

        /// <summary>
        /// This is my four parameter constructor for a spreadsheet. It takes a string as a file path first, then it's identical to the three param constructor.
        /// The filepath is used to load a xml file into a spreadsheet.
        /// Note: spreadsheet versions must match or an error will be thrown
        /// </summary>
        /// <param name="FilePath">file path that will be loaded</param>
        /// <param name="isValid">Validator supplied by user</param>
        /// <param name="normalize">Normalizer supplied by user</param>
        /// <param name="version">the version of spreadsheet.</param>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public Spreadsheet(string FilePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            spreadsheet = new();
            dependencyGraph = new();
            values = new();
            Changed = false;

            if (version != GetSavedVersion(FilePath))
            {
                throw new SpreadsheetReadWriteException("Mismatching versions, cannot load given file path");
            }
            try
            {
                XmlReaderSettings settings = new();
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(FilePath, settings))
                {
                    try
                    {
                        reader.Read();//Start
                        reader.Read();//Spreadsheet
                        reader.Read(); //cell
                        while (reader.Read()) //name
                        {
                            string name = reader.ReadElementContentAsString();// string of name
                            string contents = reader.ReadElementContentAsString(); //string of contents
                            SetContentsOfCell(Normalize(name), contents);
                            reader.ReadEndElement(); //end cell element
                        }
                        reader.Dispose();
                    }
                    catch
                    {
                        throw new SpreadsheetReadWriteException($"Error reading file {FilePath}");
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error loading spreadsheet");
            }

        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);
            if (!IsValidName(name) || !IsValid(name)) { throw new InvalidNameException(); }
            if (spreadsheet.TryGetValue(name, out var cell))
            {
                return cell;
            }
            return "";
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return spreadsheet.Keys;
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, double number)
        {
            if (spreadsheet.TryGetValue(name, out var cell))
            {
                if (GetCellContents(name) is Formula formula)
                {
                    ExtractVarsRemoveDependencies(name, formula);
                }
                spreadsheet.Remove(name);
            }
            spreadsheet.Add(name, number);
            return GetCellsToRecalculate(name).ToImmutableList();
        }

        ///  <inheritdoc/>
        protected override IList<string> SetCellContents(string name, string text)
        {
            if (spreadsheet.TryGetValue(name, out var cell))
            {
                if (GetCellContents(name) is Formula formula)
                {
                    ExtractVarsRemoveDependencies(name, formula);
                }
                spreadsheet.Remove(name);
            }
            spreadsheet.Add(name, text);
            return GetCellsToRecalculate(name).ToImmutableList();
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            IList<string> cellNames = [];
            if (spreadsheet.TryGetValue(name, out var oldCell))
            {
                if (GetCellContents(name) is Formula oldFormula)
                {
                    ExtractVarsRemoveDependencies(name, oldFormula);
                }
                spreadsheet.Remove(name);
            }
            spreadsheet.Add(name, formula);
            foreach (string variable in formula.GetVariables())
            {
                dependencyGraph.AddDependency(name, variable);
            }
            try
            {
                cellNames = GetCellsToRecalculate(name).ToImmutableList();
            }
            catch (CircularException)
            {
                spreadsheet.Remove(name);
                values.Remove(name);

                if (oldCell is not null)
                {
                    spreadsheet.Add(name, oldCell);
                    StoreCellValue(name, CalculateCellValue(name));
                }
                foreach (string variable in formula.GetVariables())
                {
                    dependencyGraph.RemoveDependency(name, variable);
                }
                throw new CircularException();
            }
            return cellNames;
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencyGraph.GetDependees(name);
        }

        /// <inheritdoc/>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);
            if (content is null) { return []; }
            if (!IsValidName(name) || !IsValid(name)) { throw new InvalidNameException(); };
            if (Double.TryParse(content, out double num))
            {
                Changed = true;
                return FacilitateRecalculation(SetCellContents(name, num));
            }
            try
            {
                if (content[0] == '=')
                {
                    Formula formula = new(content.Substring(1), Normalize, IsValid);
                    Changed = true;
                    return FacilitateRecalculation(SetCellContents(name, formula));
                }

            }
            catch (IndexOutOfRangeException) { }
            Changed = true;
            return FacilitateRecalculation(SetCellContents(name, content));
        }

        /// <inheritdoc/>
        public override string GetSavedVersion(string filename)
        {
            string foundVersion = "";
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    reader.Read();
                    if (reader.IsStartElement())
                    {
                        foundVersion = reader.GetAttribute(0);
                        reader.Dispose();
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException($"Error reading file {filename}");
            }
            return foundVersion;
        }

        /// <inheritdoc/>
        public override void Save(string filename)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    if (Changed is true)
                    {
                        writer.WriteRaw(GetXML());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                    Changed = false;
                }
            }
            catch { throw new SpreadsheetReadWriteException($"Couldn't write to file {filename}"); }
        }

        /// <inheritdoc/>
        public override string GetXML()
        {
            StringBuilder xmlStringBuilder = new();
            foreach (string name in GetNamesOfAllNonemptyCells())
            {
                xmlStringBuilder.Append("<cell>");
                xmlStringBuilder.Append("<name>");
                xmlStringBuilder.Append($"{name}");
                xmlStringBuilder.Append("</name>");
                xmlStringBuilder.Append("<contents>");
                object contents = GetCellContents(name);
                if (contents is Formula)
                {
                    xmlStringBuilder.Append($"={contents}");
                }
                else
                {
                    xmlStringBuilder.Append($"{contents}");
                }
                xmlStringBuilder.Append("</contents>");
                xmlStringBuilder.Append("</cell>");
            }
            return xmlStringBuilder.ToString();
        }

        /// <inheritdoc/>
        public override object GetCellValue(string name)
        {
            if (!IsValidName(name)) { throw new InvalidNameException(); }
            if (values.TryGetValue(name, out var value))
            {
                return value;
            }
            return "";
        }

        /// <summary>
        /// This method is called in the SetContentsOfCell method after the new cell has already been created.
        /// It is passed the IList(string) that is returned by the appropriate setcellcontents method.
        /// It then calls calculateCellValue on each cell in the list, which effectively recalculates all necessary cells. 
        /// </summary>
        /// <param name="cellsToRecalculate">The IList(string) created by appropriate SetCellContents method</param>
        /// <returns></returns>
        private IList<string> FacilitateRecalculation(IList<string> cellsToRecalculate)
        {
            foreach (string cellName in cellsToRecalculate)
            {
                CalculateCellValue(cellName);
            }
            return cellsToRecalculate;
        }

        /// <summary>
        /// This helper method is only called by CalculateCellValue method, and stores the value matched to the name in
        /// the values dictionary for lookup by the getcellvalue method
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="value">the object found in calculation to be stored. Either a double, a string, or a formula error</param>
        private void StoreCellValue(string name, object value)
        {
            if (values.TryGetValue(name, out var val))
            {
                values.Remove(name);
            }
            values.Add(name, value);
        }

        /// <summary>
        /// This helper method is called by SetContentsOfCell. It determines what the object in the cell is, and either
        /// evaluates the formula, or find a double or a string, and then calls the helper method StoreCellValue(), 
        /// which then stores the value in a dictionary(name, value)
        /// </summary>
        /// <param name="name">the name of the cell to be calculated</param>
        /// <returns>returns the object in the named cell</returns>
        private object CalculateCellValue(string name)
        {
            object content = GetCellContents(name);
            if (content is Formula formula)
            {
                object answer = formula.Evaluate(FindVarValue);
                StoreCellValue(name, answer);
                return answer;
            }
            else if (content is double num)
            {
                StoreCellValue(name, num);
                return num;
            }
            else
            {
                StoreCellValue(name, content);
                return content;
            }
        }

        /// <summary>
        /// This method serves as the delegate passed into Evaluate when evaluating a formula.
        /// It gets all of the non-empty cell names, then determins if the cell exists, if it does, it returns that value
        /// </summary>
        /// <param name="name">the name of the variable (cell name)</param>
        /// <returns>a double that is looked up correctly in the spreadsheet</returns>
        /// <exception cref="ArgumentException">This is thrown if the value cannot be looked up successfully. </exception>
        private double FindVarValue(string name)
        {
            HashSet<string> vars = GetNamesOfAllNonemptyCells().ToHashSet();
            if (vars.Contains(name))
            {
                object response = GetCellValue(name);
                if (response is double value)
                {
                    return value;
                }
                throw new ArgumentException();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// This helper method confirms valid syntax for a name, first checks that the first char is either a letter or underscore, 
        /// then checks for only letters, digits, or underscores in the remaining chars
        /// </summary>
        /// <param name="name">The "name" of the cell to be checked</param>
        /// <returns>Returns a bool if the cell name is valid or not</returns>
        private static bool IsValidName(string name)
        {
            if (name is null || name.Count() < 2) { return false; }
            if (!Char.IsLetter(name[0]))
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!Char.IsLetter(c) && !Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This method is used in SetCellContents methods when a formula is being replaced.
        /// It extracts the old variables from the old formula, then for each var, RemoveDependency
        /// from the name of the cell and variable
        /// Note: Here, we refer to other cell names as vars, as named in our formula class
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="formula">the old formula being removed</param>
        private void ExtractVarsRemoveDependencies(string name, Formula formula)
        {
            IEnumerable<string> vars = formula.GetVariables();
            foreach (string var in vars)
            {
                dependencyGraph.RemoveDependency(name, var);
            }
        }
    }
}

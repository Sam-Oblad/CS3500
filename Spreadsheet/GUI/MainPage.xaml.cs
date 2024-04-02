/// <summary>
/// Authors: Araum Karimi, Sam Oblad
/// Date: 3/3/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500, Araum Karimi, Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// We, Araum Karimi and Sam Oblad, certify that we wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in the solution README file.
/// 
/// File Contents:
/// This file contains our mainpage class for the GUI project. It inherits from ContentPage. 
/// </summary>
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Our mainpage class inherits from the ContentPage class. Our whole gui program is run on this one page.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// A constant value that represents the amount of columns in the spreadsheet, this allows for easy resizing of loops
        /// </summary>
        private const int columns = 10;

        /// <summary>
        /// A constant value that represents the amount of rows in the spreadsheet, this allows for easy resizing of loops
        /// </summary>
        private const int rows = 10;

        /// <summary>
        /// Our spreadsheet class data structure
        /// </summary>
        private Spreadsheet ss = new(s=>true, s=>s.ToUpper(), "six");

        /// <summary>
        /// Stores dynamically allocated entries
        /// </summary>
        private Dictionary<String, Entry> entryDict = new();

        /// <summary>
        /// utilized to track undo/redo information
        /// </summary>
        private ChangesTracker changesTracker = new();

        /// <summary>
        /// Stores a reference to the current cell
        /// </summary>
        private Entry prevCell;

        /// <summary>
        /// Stores a reference to the current cell
        /// </summary>
        private Entry currentCell;

        /// <summary>
        /// This is the help page object that is displayed when get help is clicked
        /// </summary>
        private Page helpPage;

        /// <summary>
        /// Used to check if the same file path has been used, bypassing a need to ask to overwrite a file
        /// </summary>
        private string lastSaveFilePath;

        /// <summary>
        /// This is the constructor for our main page. it utilizes loops and our const rows/columns values to generate a spreadsheet
        /// populated with entry elements.We assigned the entry elements four different event functions upon creation, and then place them into
        /// a border which is then in turn placed into a row of (int column) elements. We do that (int rows) amount of times. The left and top
        /// labels are also built here depending on our const int values. 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            helpPage = new HelpPage();
            for (int i = 0; i < columns; i++)
            {
                TopLabels.Add(
                    new Border
                    {
                        Padding = 0,
                        Margin = 0,
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 2,
                        HeightRequest = 30,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                            new Label
                            {
                                Text = $"{(char)('A' + i)}",
                                BackgroundColor = Colors.Lavender,
                                TextColor = Colors.Black,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center
                            }
                    }
                );
            }

            for(int i = 0; i < rows; i++)
            {               
                LeftLabels.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 2,
                        HeightRequest = 30,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Start,
                        Content =
                            new Label
                            {
                                Text = $"{i + 1}",
                                TextColor = Colors.Black,
                                BackgroundColor = Colors.Lavender,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center
                            }
                    }
                );

                HorizontalStackLayout row = new();
                row.Padding = 0;
                row.Spacing = 0;
                
                for (int j = 0; j < columns; j++)
                {
                    Entry entry = new Entry
                    {
                        
                        AutomationId = ((char)('A' + j)).ToString() + (i + 1),
                        TextColor = Colors.Black,
                        Margin = 0,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        BackgroundColor = Colors.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Start,
                    };
                    entry.Completed += SetCellFromGrid;
                    entry.Focused += SelectCell;
                    entry.Unfocused += DeselectCell;
                    entryDict.Add(entry.AutomationId, entry);      

                    row.Add(
                        new Border
                        {
                            Margin = 0,
                            Stroke = Color.FromRgb(0, 0, 0),
                            StrokeThickness = 1,
                            HeightRequest = 30,
                            WidthRequest = 75,
                            Padding = 0,
                            HorizontalOptions = LayoutOptions.Center,
                            Content = entry
                        }
                    );
                }
                Grid.Add(row);
            }
        }

        /// <summary>
        /// This function is called upon successful loading of the page right after construction. It selects A1 as the focused cell,
        /// then it sets the widget values appropriately
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void OnPageLoaded(object sender, EventArgs e)
        {
            // set the focus to the widget (e.g., entry) that you want   
            entryDict.TryGetValue("A1", out Entry? firstEntry);
            firstEntry.Focus();
            firstEntry.BackgroundColor = Colors.Lavender;
            currentCellWidget.Text = "A1";
            cellValueWidget.Text = ss.GetCellValue(firstEntry.AutomationId).ToString();
            cellContentsWidget.Text = ss.GetCellContents(firstEntry.AutomationId).ToString();
        }

        /// <summary>
        /// This method keeps the current cell highlighted only when the focus shifts from the cell into
        /// the editing entry widget at the top. It also keeps the cell contents visible instead of the value.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void HighlightCell(object? sender, EventArgs e)
        {
            try
            {
                entryDict.TryGetValue(currentCellWidget.Text, out Entry? entry);
                prevCell = entry;
                entry.BackgroundColor = Colors.Lavender;
                entry.Text = CellContentsToString(ss.GetCellContents(currentCellWidget.Text));
            }
            catch { }
        }
        
        /// <summary>
        /// This method removes the highlight of the previous cell if the entry editing widget was focused and lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveHighlight(object? sender, EventArgs e)
        {
            if (prevCell is not null)
            {
                    prevCell.Text = ss.GetCellValue(prevCell.AutomationId).ToString();
                    DetectError(prevCell);
                    prevCell.BackgroundColor = Colors.White;
            }
        }

        /// <summary>
        /// Change the color of a cell upon selection
        /// </summary>
        /// <param name="sender">The selected cell</param>
        /// <param name="e">unused</param>
        private void SelectCell(object? sender, EventArgs e)
        {
            if (sender is Entry cell)
            {
                currentCell = cell;
                currentCellWidget.Text = cell.AutomationId;
                string? value = ss.GetCellValue(cell.AutomationId).ToString();
                cellValueWidget.Text = value == "SpreadsheetUtilities.FormulaError" ? "#Error" : value;
                cellContentsWidget.Text = CellContentsToString(ss.GetCellContents(currentCellWidget.Text));
                cell.Text = cellContentsWidget.Text;
                cell.BackgroundColor = Colors.Lavender;
                if (prevCell is not null && prevCell != cell) { prevCell.BackgroundColor = Colors.White; }
            }
        }

        /// <summary>
        /// Changes the color of a cell upon deselection
        /// </summary>
        /// <param name="sender">The cell deselected</param>
        /// <param name="e">Unused</param>
        private void DeselectCell(object? sender, EventArgs e)
        {
            if (sender is Entry send)
            {
                send.Text = ss.GetCellValue(send.AutomationId).ToString();
                DetectError(sender);
                prevCell = send;
            }
        }

        /// <summary>
        /// This sets the contents of the cell using an entry element within the grid. This method also sets focus onto 
        /// the next cell in the current column. If the selection is at the bottom of a column, the focus does not change. 
        /// This method first extracts the name of the cell, creates a Change instance used for undo/redo, and then it calculates the new value/content and updates the widgets appropriately. It
        /// also selects the next cell so pressing enter in the grid will assign the cell and then jump down one. 
        /// Also checks and catches circular/formulaFormat exceptions
        /// </summary>
        /// <param name="sender">The entry element used</param>
        /// <param name="e">Unused</param>
        private async void SetCellFromGrid(object? sender, EventArgs e)
        {
            if(sender is Entry send)
            {
                try
                {
                    string cellName = send.AutomationId;
                    string cellContentsString = CellContentsToString(ss.GetCellContents(cellName));
                    StartChangeTracker(cellName, cellContentsString, send.Text);
                    DisplayNewCellValues(ss.SetContentsOfCell(cellName, send.Text));
                    cellValueWidget.Text = ss.GetCellValue(cellName).ToString();
                    cellContentsWidget.Text = CellContentsToString(ss.GetCellContents(cellName));
                    DetectError(sender);

                    int.TryParse(cellName.Substring(1), out int num);
                    string nextCellName = cellName[0] + (num + 1).ToString();
                    entryDict.TryGetValue(nextCellName, out Entry? nextCell);
                    if (nextCell is not null)
                    {
                        nextCell.Focus();
                    }
                }
                catch (CircularException)
                {
                    await DisplayAlert("Circular Dependency Detected", $"The formula '{send.Text}' will create a circular dependency", "OK");
                    changesTracker.NewChange();
  
                }
                catch (FormulaFormatException)
                {
                    await DisplayAlert("Invalid Formula Detected", $"The formula '{send.Text} is invalid", "OK");
                    changesTracker.NewChange();
                }
            }
        }

        /// <summary>
        /// Sets the cell contents from the widget bar entry element. First it extracts the name, creates a change instance for undo redo, then calculates 
        /// new cell value/contents and updates widgets appropriately. Also checks and catches circular/formulaFormat exceptions
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private async void SetCellFromWidget(object? sender, EventArgs e)
        {
            if(sender is Entry send)
            {
                try
                {
                    string cellName = currentCell.AutomationId;
                    string cellContentsString = CellContentsToString(ss.GetCellContents(cellName));
                    StartChangeTracker(cellName, cellContentsString, send.Text);
                    DisplayNewCellValues(ss.SetContentsOfCell(cellName, send.Text));
                    entryDict.TryGetValue(cellName, out Entry? currCell);
                    currCell.Text = CellContentsToString(ss.GetCellContents(cellName));
                    string? value = ss.GetCellValue(cellName).ToString();
                    cellValueWidget.Text = value == "SpreadsheetUtilities.FormulaError" ? "#Error" : value;
                }
                catch (CircularException)
                {
                    await DisplayAlert("Circular Dependency Detected", $"The formula '{send.Text}' will create a circular dependency", "OK");
                    changesTracker.NewChange();
                }
                catch (FormulaFormatException)
                {
                    await DisplayAlert("Invalid Formula Detected", $"The formula '{send.Text} is invalid", "OK");
                    changesTracker.NewChange();
                }
            }
        }

        /// <summary>
        /// This is called when hitting file, then new in the menu bar. First it sets widgets appropriately, and then if it has changed, it asks for confirmation before overwriting
        /// unsaved changes. if there isn't any changes, then it just creates a new spreadsheet.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private async void FileMenuNew(object sender, EventArgs e)
        {
            currentCellWidget.Text = "A1";
            entryDict.TryGetValue("A1", out Entry? firstEntry);
            firstEntry.Focus();
            firstEntry.BackgroundColor = Colors.Lavender;
            if (ss.Changed)
            {
                bool answer = await DisplayAlert("Unsaved Changes Detected", "Do you wish to overwrite the current spreadsheet?", "yes", "no");
                if (answer)
                {
                    DeleteAllEntries();
                    ss = new(s => true, s => s.ToUpper(), "six");
                    changesTracker.Reset();
                    lastSaveFilePath = "";
                    RedoButton.IsEnabled = false;
                    UndoButton.IsEnabled = false;
                }
            }
            else
            {
                DeleteAllEntries();
                ss = new(s => true, s => s.ToUpper(), "six");
                changesTracker.Reset();
                lastSaveFilePath = "";
                RedoButton.IsEnabled = false;
                UndoButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// This method is called when the file menu selection is open. It checks that its okay to overwrite the file, then it lets the user select a file using file picker. 
        /// After, it loads that file into the spreadsheet, or it throws an error if unable to do so. Finally, I initialize the spreadsheet by modifying A1 to its current value, so that the 
        /// ss changes, which will allow the user to immediately save else where if necessary
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private async void FileMenuOpenAsync(object sender, EventArgs e)
        {
            bool flag = false;
            if (ss.Changed)
            {
                bool answer = await DisplayAlert("Unsaved Changes Detected", "Do you wish to overwrite the current spreadsheet?", "yes", "no");
                if (answer) { flag = true; }
            }
            else { flag = true; }

            if (flag)
            {
                DeleteAllEntries();
                var file = await FilePicker.PickAsync();
                try
                {
                    if (file is not null)
                    {
                        changesTracker.Reset();
                        RedoButton.IsEnabled = false;
                        UndoButton.IsEnabled = false;

                        ss = new Spreadsheet(file.FullPath, (s) => true, (s) => s.ToUpper(), "six");
                        foreach (string name in ss.GetNamesOfAllNonemptyCells())
                        {
                            if (entryDict.TryGetValue(name, out Entry? currCell))
                            {
                                currCell.Text = ss.GetCellValue(name).ToString();
                                DetectError(currCell);
                            }
                        }
                        string a1String = CellContentsToString(ss.GetCellContents("A1"));
                        ss.SetContentsOfCell("A1", a1String);
                        lastSaveFilePath = "";
                    }
                }
                catch
                {
                    await DisplayAlert("Unable to Load file", "The input file should be a version 'six' spreadsheet file (ss.sprd)", "OK");
                }
            }
        }

        /// <summary>
        /// This method is called in the file method when save is hit. First it checks for changes, if there isnt, it makes a change such that the user can always save if they want to. 
        /// Then it asks for a file path, and it checks if the path already exists and that the file path is not the same as the last saved file path. if it exists, but is not the most recent save,
        /// it asks to overwrite. otherwise it saves to the file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileSave(object sender, EventArgs e)
        {
            if (!ss.Changed) { ss.SetContentsOfCell("A1", ss.GetCellContents("A1").ToString()); }
            string filePath = await DisplayPromptAsync("Save As", "Enter a filepath to save to, use file type '.sprd'", "OK", "Cancel", null, -1, null, ".sprd" );
            bool overwrite = false;
            if (File.Exists(filePath) && filePath != lastSaveFilePath)
            {
                overwrite = await DisplayAlert("Overwrite File?", $"{filePath}", "OK", "Cancel");
            }
            else
            {
                overwrite = true;
            }

            if (overwrite)
            {
                try
                {
                    if (filePath != null) { ss.Save(filePath); }
                    lastSaveFilePath = filePath;
                }
                catch (SpreadsheetReadWriteException) { await DisplayAlert("Error during saving", $"Could not write to file {filePath}, Check your file path", "OK"); }
            }
        }

        ///<summary>
        /// This helper method opens the help menu, it also removes highlight from prevCell
        ///</summary>
        private async void OpenHelpMenu(object sender, EventArgs e)
        {
            await Navigation.PushAsync(helpPage);
            prevCell.BackgroundColor = Colors.White;
        }

        /// <summary>
        /// This helper method detects formula errors when setting a cells contents, and notifies the user appropriately by changing the string so it makes sense to the user
        /// </summary>
        /// <param name="sender">unused</param>
        private void DetectError(object? sender)
        {
            if (sender is Entry entry)
            {
                if (entry.Text == "SpreadsheetUtilities.FormulaError")
                {
                    entry.Text = "#Error";
                }
                if(entry.Text == "#Error")
                {
                    cellValueWidget.Text = "#Error";
                }
            }
        }

        /// <summary>
        /// This helper method deletes all the Non-empty cells in this spreadsheet
        /// </summary>
        private void DeleteAllEntries()
        {
            foreach(Entry entry in entryDict.Values)
            {
                entry.Text = null;
                currentCellWidget.Text = null;
                cellValueWidget.Text = null;
                cellContentsWidget.Text = null;
            }
        }

        /// <summary>
        /// This helper method displays new cell values after a change has been made, whether setting, undoing, or redoing
        /// </summary>
        /// <param name="cellNames"> List of cells that should be redisplayed </param>
        private void DisplayNewCellValues(IList<string> cellNames)
        {
            foreach(string cellName in cellNames)
            {
                if(entryDict.TryGetValue(cellName, out Entry? currCell))
                {
                    currCell.Text = ss.GetCellValue(currCell.AutomationId).ToString();
                    DetectError(currCell);
                }
            }
        }

        /// <summary>
        /// This method will undo the most recent change made to the spreadsheet. It uses data from the changes tracker to
        /// determine what cell should be changed, as well as what it should be changed to. If there are no changes to undo,
        /// the undo button is disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Undo(object sender, EventArgs e)
        {
            try
            {
                Change undo = changesTracker.GoBack();
                string cellName = undo.Name;
                DisplayNewCellValues(ss.SetContentsOfCell(cellName, undo.CachedContent));
                UndoRedoHelper(cellName);
                RedoButton.IsEnabled = changesTracker.CanGoForward();
                UndoButton.IsEnabled = changesTracker.CanGoBack();
                entryDict.TryGetValue(cellName, out Entry? currCell);
                currCell.Focus();
                currentCell = currCell;
            }
            catch (Exception)
            {
                RedoButton.IsEnabled = changesTracker.CanGoForward();
                UndoButton.IsEnabled = changesTracker.CanGoBack();
            }
            if (currentCell != prevCell)
            {
                currentCell.BackgroundColor = Colors.Lavender;
                prevCell.BackgroundColor = Colors.White;
            }

        }

        /// <summary>
        /// This method will redo the most recent undo made to the spreadsheet. It uses data from the changes tracker to
        /// determine what cell should be changed, as well as what it should be changed to. If there are no changes to redo,
        /// the redo button is disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo(object sender, EventArgs e)
        {
            try
            {
                Change redo = changesTracker.GoForward();
                string cellName = redo.Name;
                DisplayNewCellValues(ss.SetContentsOfCell(cellName, redo.NewContent));
                UndoRedoHelper(cellName);
                RedoButton.IsEnabled = changesTracker.CanGoForward();
                UndoButton.IsEnabled = changesTracker.CanGoBack();
                entryDict.TryGetValue(cellName, out Entry? currCell);
                currCell.Focus();
                currentCell = currCell;
            } catch (Exception)
            {
                RedoButton.IsEnabled = changesTracker.CanGoForward();
                UndoButton.IsEnabled = changesTracker.CanGoBack();
            }
            if (currentCell != prevCell)
            {
                currentCell.BackgroundColor = Colors.Lavender;
                prevCell.BackgroundColor = Colors.White;
            }
    }
        /// <summary>
        /// Helper method for updating cells in the GUI when undo or redo is called
        /// </summary>
        /// <param name="cellName"> Name of the cell to be updated </param>
        private void UndoRedoHelper(string cellName)
        {
            entryDict.TryGetValue(cellName, out Entry? currCell);
            object contents = ss.GetCellContents(cellName);
            cellContentsWidget.Text = contents is Formula ? "=" + contents.ToString() : contents.ToString();
            cellValueWidget.Text = ss.GetCellValue(currentCellWidget.Text).ToString();
            currentCellWidget.Text = cellName;
            currCell.Text = ss.GetCellValue(currentCellWidget.Text).ToString();
        }

        /// <summary>
        /// takes a cell contents object and transforms it into a string, this only matters for formulas
        /// </summary>
        /// <param name="contents">the contents of a cell</param>
        /// <returns>a string representing the contents</returns>
        private string CellContentsToString(object contents)
        {
            string cellContentsString;
            if (contents is Formula)
            {
                cellContentsString = "=" + contents.ToString();
            }
            else
            {
                cellContentsString = contents.ToString();
            }
            return cellContentsString;
        }

        /// <summary>
        /// starts a change class that helps with undo and redo stacks to keep track of changes
        /// </summary>
        /// <param name="cellName">the name of the cell</param>
        /// <param name="cellContents">a string representation of the cell contents</param>
        /// <param name="newContent">What the cell contents is being updated to</param>
        private void StartChangeTracker(string cellName, string cellContents, string newContent)
        {
            changesTracker.NewChange(new Change(cellName, cellContents, newContent));
            RedoButton.IsEnabled = changesTracker.CanGoForward();
            UndoButton.IsEnabled = changesTracker.CanGoBack();
        }
    }
}


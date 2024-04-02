/// <summary>
/// Author: Sam Oblad
/// Partner: None
/// Date: 2/10/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// I, Sam Oblad, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source. All
/// references used in the completion of the assignments are cited 
/// in my README file.
/// 
/// File Contents
/// This file contains my test cases for my spreadsheet class
/// </summary>
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
namespace SpreadsheetTests
{
    [TestClass]

    public class SpreadsheetTests
    {
        /// <summary>
        /// simple test creating spreadsheet
        /// </summary>
        [TestMethod]
        public void simpleSpreadTest()
        {
            Spreadsheet spreadsheet = new();
            IEnumerable<string> response = spreadsheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, response.Count());
        }

        /// <summary>
        /// Tests if a circularException is thrown correctly when a dependency loop is created
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependencyExceptionTest1()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=A1 + 5");
        }

        /// <summary>
        /// Tests if a circularException is thrown correctly when a dependency loop is created
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependencyExceptionTest2()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=B1 + 4");
            spreadsheet1.SetContentsOfCell("B1", "=A1 *2");
        }

        /// <summary>
        /// Tests if a circularException is thrown correctly when a dependency loop is created
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependencyExceptionTest3()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=B1 + 2");
            spreadsheet1.SetContentsOfCell("B1", "=C1 + 4");
            spreadsheet1.SetContentsOfCell("C1", "=D1 + 3");
            spreadsheet1.SetContentsOfCell("D1", "=A1 * 2");
        }

        /// <summary>
        /// Tests if a spreadsheet is unchanged when a circularException is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependencyExceptionTest4()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=B1 + 2");
            spreadsheet1.SetContentsOfCell("B1", "=C1 + 4");
            spreadsheet1.SetContentsOfCell("C1", "=D1 + 3");
            spreadsheet1.SetContentsOfCell("D1", "=A1 * 2");
            Assert.AreEqual(3, spreadsheet1.GetNamesOfAllNonemptyCells().Count());
        }

        //!These tests are for the setCellContents methods with variour parameter types
        /// <summary>
        /// Tests setting cell contents using a double
        /// </summary>
        [TestMethod]
        public void setCellContentsDoubleTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "12.01");
            Assert.IsTrue(spreadsheet1.GetCellContents("A1") is 12.01);
        }

        /// <summary>
        /// Tests setting cell contents using a string
        /// </summary>
        [TestMethod]
        public void setCellContentsStringTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "Hello world");
            Assert.IsTrue(spreadsheet1.GetCellContents("B1") is "Hello world");
        }

        /// <summary>
        /// Tests setting cell contents using a formula
        /// </summary>
        [TestMethod]
        public void setCellContentsFormulaTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("C1", "=X1 + Y1");
            Assert.IsTrue(spreadsheet1.GetCellContents("C1") is Formula);
        }

        /// <summary>
        /// Tests empty string name on a double construction
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setEmptyStringContentsDoubleTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("", "12.01");
        }

        /// <summary>
        /// Tests empty string name on a string construction
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setEmptyStringContentsStringTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("", "Hello");
        }

        /// <summary>
        /// Tests empty string name on a formula construction
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setEmptyStringContentsFormulaTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("", "=X+1");
        }

        /// <summary>
        /// Tests setting cell contents using a double, empty cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setInvalidCellContentsDoubleTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("1X", "12.01");
        }

        /// <summary>
        /// Tests setting cell contents using a string, incorrect cell name syntax
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setInvalidCellContentsStringTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("1X", "Hello world");
        }

        /// <summary>
        /// Tests setting cell contents using a string, null text argument
        /// </summary>
        [TestMethod]
        public void setNullArgumentCellContentsStringTest()
        {
            Spreadsheet spreadsheet1 = new();
            string text = null;
            spreadsheet1.SetContentsOfCell("X1", text);
            Assert.AreEqual("", spreadsheet1.GetCellContents("X1"));
        }

        /// <summary>
        /// Tests setting cell contents using a formula, null as cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setInvalidCellContentsFormulaTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell(null, "=X + Y");
        }

        /// <summary>
        /// Tests setting cell contents using a formula, null as cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentsInvalidTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=X1 + Y1");
            spreadsheet1.GetCellContents("");
        }

        /// <summary>
        /// Tests setting cell contents using a formula, null as cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1%$$#%", "=X1 + Y1");
        }

        /// <summary>
        /// Tests trying to get contents of an empty cell
        /// </summary>
        [TestMethod]
        public void getCellContentsEmptyCellTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=X1 + Y1");
            object response = spreadsheet1.GetCellContents("B1");
            Assert.AreEqual("", response);
        }

        /// <summary>
        /// Tests getting all populated cells, returned as an IEnumerable
        /// </summary>
        [TestMethod]
        public void getAllNonEmptyCellsTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "12.01");
            spreadsheet1.SetContentsOfCell("B1", "Hello world");
            spreadsheet1.SetContentsOfCell("C1", "=X1 + Y1");
            HashSet<string> answers = new() { "A1", "B1", "C1" };
            Assert.IsTrue(answers.SequenceEqual(spreadsheet1.GetNamesOfAllNonemptyCells()));
        }

        /// <summary>
        /// Tests that getCellContents() is returning the appropriate value of the cells
        /// tests string, double, and formula values 
        /// </summary>
        [TestMethod]
        public void getCellContentsTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "420.69");
            spreadsheet1.SetContentsOfCell("B1", "Hello World!");
            spreadsheet1.SetContentsOfCell("Z2", "=5e2 + 1");

            object answer = spreadsheet1.GetCellContents("A1");
            Assert.AreEqual(420.69, answer);

            answer = spreadsheet1.GetCellContents("B1");
            Assert.AreEqual("Hello World!", answer);

            answer = spreadsheet1.GetCellContents("Z2");
            Assert.AreEqual(new Formula("5e2 + 1"), answer);
        }

        /// <summary>
        /// Tests that SetCellContents is returning appropriate values, note that SetCellContents returns direct and indirect dependents, 
        /// in this case, "A1" only has direct dependents
        /// </summary>
        [TestMethod]
        public void getDirectDependentsTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "=A1 * A1");
            spreadsheet1.SetContentsOfCell("C1", "=B1 + A1");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3");

            HashSet<string> answers = new() { "A1", "B1", "C1" };
            Assert.IsTrue(answers.SequenceEqual(response));
        }

        /// <summary>
        /// Tests that SetCellContents is returning appropriate values and order for all dependencies of A1
        /// </summary>
        [TestMethod]
        public void getCorrectReturnSetContentsOfCellTest1()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "=A1 * C1");
            spreadsheet1.SetContentsOfCell("C1", "=2 + A1");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3 * 3");
            HashSet<string> answers = new() { "A1", "C1", "B1" };
            Assert.IsTrue(answers.SequenceEqual(response));
        }


        /// <summary>
        /// Tests that SetCellContents is returning appropriate values and order for all dependencies of A1
        /// </summary>
        [TestMethod]
        public void getCorrectReturnSetContentsOfCellTest2()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "=A1 * A1");
            spreadsheet1.SetContentsOfCell("C1", "=B1 + D1");
            spreadsheet1.SetContentsOfCell("D1", "=A1 - B1");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3");
            HashSet<string> answers = new() { "A1", "B1", "D1", "C1" };
            Assert.IsTrue(answers.SequenceEqual(response));
        }

        /// <summary>
        /// Tests that SetCellContents is returning appropriate values and order for all dependencies of A1
        /// </summary>
        [TestMethod]
        public void getCorrectReturnSetContentsOfCellTest3()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "=A1 * A1");
            spreadsheet1.SetContentsOfCell("C1", "=B1 + X1");
            spreadsheet1.SetContentsOfCell("D1", "=A1 - B1");
            spreadsheet1.SetContentsOfCell("X1", "=A1 - Z1");
            spreadsheet1.SetContentsOfCell("Y1", "=A1 - C1");
            spreadsheet1.SetContentsOfCell("Z1", "=B1 - D1");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3");
            HashSet<string> answers = new() { "A1", "B1", "D1", "Z1", "X1", "C1", "Y1" };
            Assert.IsTrue(answers.SequenceEqual(response));
        }

        /// <summary>
        /// Tests that SetCellContents is returning appropriate values and order for all dependencies of X1
        /// NOTE: after playing around with the ordering on this test, it would seem that GetCellsToBeRecalculated decides priorities
        /// based on when the cell was set in reference to the other cells, however, it always returns the cells that need to be calculated 
        /// before other cells. For example, if setContents of C1 takes place at the bottom, R1 and Q1 are near the end of the recalculation,
        /// but if setContents of C1 takes place where it does below, R1 and Q1 are recalculated immediately. Since they have no effect on the rest
        /// of the formulas, it doesn't matter where they are calculated in reference to the other formulas. 
        /// </summary>
        [TestMethod]
        public void getCorrectReturnSetContentsOfCellTest4()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("A1", "=7 * Y1");
            spreadsheet1.SetContentsOfCell("B1", "=6 * A1");
            spreadsheet1.SetContentsOfCell("C1", "=5 + X1");
            spreadsheet1.SetContentsOfCell("D1", "=4 - B1");
            spreadsheet1.SetContentsOfCell("Q1", "=X1 - X1");
            spreadsheet1.SetContentsOfCell("R1", "=X1 + X1 * X1");
            spreadsheet1.SetContentsOfCell("X1", "=3 - 10");
            spreadsheet1.SetContentsOfCell("Y1", "=2 - C1");
            spreadsheet1.SetContentsOfCell("Z1", "=1 - D1");

            IList<string> response = spreadsheet1.SetContentsOfCell("X1", "3");
            HashSet<string> answers = new() { "X1", "R1", "Q1", "C1", "Y1", "A1", "B1", "D1", "Z1" };
            Assert.IsTrue(answers.SequenceEqual(response));
        }

        /// <summary>
        /// Tests that dependencyGraph is being updated appropriately when changing a cell containing
        /// a formula, also asserts that values in the spreadsheet are changing correctly
        /// </summary>
        [TestMethod]
        public void changeFormulaCellTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "=A1 * A1");
            spreadsheet1.SetContentsOfCell("C1", "=B1 + A1");
            spreadsheet1.SetContentsOfCell("D1", "=B1 - C1");
            spreadsheet1.SetContentsOfCell("E1", "=2 + 2");
            spreadsheet1.SetContentsOfCell("F1", "=E1 + 2");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3");

            HashSet<string> answers = new() { "A1", "B1", "C1", "D1" };
            Assert.IsTrue(answers.SequenceEqual(response));

            spreadsheet1.SetContentsOfCell("B1", "10");
            spreadsheet1.SetContentsOfCell("D1", "Hello World!");
            spreadsheet1.SetContentsOfCell("F1", "=A1 * 2");
            response = spreadsheet1.SetContentsOfCell("A1", "4");
            answers = new() { "A1", "C1", "F1" };
            Assert.IsTrue(answers.SequenceEqual(response));
            Assert.IsTrue(spreadsheet1.GetCellContents("F1") is Formula);
            Assert.IsTrue(spreadsheet1.GetCellContents("D1") is "Hello World!");
            Assert.IsTrue(spreadsheet1.GetCellContents("B1") is 10.0);
        }

        /// <summary>
        /// Tests that dependencyGraph is being updated appropriately when changing a cell containing
        /// a double, also asserts that values within spreadsheet are changing correctly
        /// </summary>
        [TestMethod]
        public void changeDoubleCellTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "1.1");
            spreadsheet1.SetContentsOfCell("C1", "2.2");
            spreadsheet1.SetContentsOfCell("D1", "3.3");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "3");

            HashSet<string> answers = new() { "A1" };
            Assert.IsTrue(answers.SequenceEqual(response));


            spreadsheet1.SetContentsOfCell("B1", "=A1+5");
            spreadsheet1.SetContentsOfCell("D1", "Hello World!");
            response = spreadsheet1.SetContentsOfCell("A1", "4");
            answers = new() { "A1", "B1" };
            Assert.IsTrue(answers.SequenceEqual(response));
            Assert.IsTrue(spreadsheet1.GetCellContents("B1") is Formula);
            Assert.IsTrue(spreadsheet1.GetCellContents("D1") is "Hello World!");
            Assert.IsTrue(spreadsheet1.GetCellContents("C1") is 2.2);
        }

        /// <summary>
        /// Tests that dependencyGraph is being updated appropriately when changing a cell containing
        /// a string, also asserts that values within spreadsheet are changing correctly
        /// </summary>
        [TestMethod]
        public void changeStringCellTest()
        {
            Spreadsheet spreadsheet1 = new();
            spreadsheet1.SetContentsOfCell("B1", "Hello");
            spreadsheet1.SetContentsOfCell("C1", "World");
            spreadsheet1.SetContentsOfCell("D1", "!");
            IList<string> response = spreadsheet1.SetContentsOfCell("A1", "Whaddup");

            HashSet<string> answers = new() { "A1" };
            Assert.IsTrue(answers.SequenceEqual(response));


            spreadsheet1.SetContentsOfCell("B1", "=A1+5");
            spreadsheet1.SetContentsOfCell("D1", "2.2");
            response = spreadsheet1.SetContentsOfCell("A1", "4");
            answers = new() { "A1", "B1" };
            Assert.IsTrue(answers.SequenceEqual(response));
            Assert.IsTrue(spreadsheet1.GetCellContents("B1") is Formula);
            Assert.IsTrue(spreadsheet1.GetCellContents("D1") is 2.2);
            Assert.IsTrue(spreadsheet1.GetCellContents("C1") is "World");
        }

        /// <summary>
        /// Massive test that makes sure setting, and getting is working
        /// also ensures that dependency graph is updating properly, ensures multiple spreadsheets are separate
        /// </summary>
        [TestMethod]
        public void stressTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("B1", "2");
            sheet1.SetContentsOfCell("A1", "1");
            sheet1.SetContentsOfCell("C1", "3");
            sheet1.SetContentsOfCell("D1", "4");
            sheet1.SetContentsOfCell("E1", "5");
            sheet1.SetContentsOfCell("F1", "6");
            sheet1.SetContentsOfCell("a1", "7");
            sheet1.SetContentsOfCell("b1", "8");
            sheet1.SetContentsOfCell("c1", "9");
            sheet1.SetContentsOfCell("d1", "10");
            sheet1.SetContentsOfCell("e1", "11");

            sheet1.SetContentsOfCell("G1", "string");
            sheet1.SetContentsOfCell("H1", "bacon");
            sheet1.SetContentsOfCell("I1", "ham");
            sheet1.SetContentsOfCell("J1", "eggs");
            sheet1.SetContentsOfCell("g1", "hashbrown");
            sheet1.SetContentsOfCell("h1", "test");
            sheet1.SetContentsOfCell("i1", "bruh");
            sheet1.SetContentsOfCell("j1", "ok");
            
            sheet1.SetContentsOfCell("K1", "=A1 + 10");
            sheet1.SetContentsOfCell("L1", "=A1 + 9");
            sheet1.SetContentsOfCell("M1", "=L1 + 10");
            IEnumerable<string> response = sheet1.SetContentsOfCell("A1", "2");
            HashSet<string> answers = new() { "A1", "L1", "M1", "K1" };
            Assert.IsTrue(answers.SequenceEqual(response));

            Spreadsheet sheet2 = new();
            response = sheet2.SetContentsOfCell("A1", "10");
            answers = new() { "A1" };
            Assert.IsTrue(answers.SequenceEqual(response));
            Assert.IsTrue(sheet2.GetCellContents("A1") is 10.0);
            Assert.IsTrue(sheet1.GetCellContents("A1") is 2.0);
        }

        /// <summary>
        /// Stress tests saving and loading spreadsheet programs with 10k cells
        /// </summary>
        [TestMethod]
        public void stressTest2()
        {
            Spreadsheet sheet1 = new Spreadsheet();
            for (int i = 1; i < 10000; i++)
            {
                sheet1.SetContentsOfCell("A" + i, "=2" + (i + 1));
            }
            sheet1.Save("stressTest1");
            Spreadsheet sheet2 = new Spreadsheet("stressTest1", s => true, s => s, "default") ;
            Assert.AreEqual(sheet1.GetCellValue("A1"), sheet2.GetCellValue("A1"));
            Assert.AreEqual(sheet1.GetCellValue("A500"), sheet2.GetCellValue("A500"));
            Assert.AreEqual(sheet1.GetCellValue("A5000"), sheet2.GetCellValue("A5000"));
            Assert.AreEqual(sheet1.GetCellValue("A9999"), sheet2.GetCellValue("A9999"));
        }

        /// <summary>
        /// Stress tests dependencies by calculating 300 cells values all the way back to the first one.
        /// </summary>
        [TestMethod]
        public void stressTest3()
        {
            Spreadsheet sheet1 = new Spreadsheet();
            for (int i = 1; i < 300; i++)
            {
                sheet1.SetContentsOfCell("A" + i, "=A" + (i+1));
            }
            sheet1.SetContentsOfCell("A299", "10");
            Assert.IsTrue(sheet1.GetCellValue("A1") is 10.0);
        }

        //!This Section tests the three parameter spreadsheet constructor
        /// <summary>
        /// basic spreadsheet creation test
        /// </summary>
        [TestMethod]
        public void threeParamSpreadsheetTest1()
        {
            Spreadsheet sheet1 = new(s => true, s => s.ToUpper(), "1.0");
            sheet1.SetContentsOfCell("x1", "hello");
            Assert.AreEqual("hello", sheet1.GetCellContents("X1"));
            Assert.AreEqual("hello", sheet1.GetCellContents("x1"));
        }

        /// <summary>
        /// basic spreadsheet creation test, should throw error for invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void threeParamSpreadsheetTest2()
        {
            Spreadsheet sheet1 = new(s => true, s => s.ToUpper(), "1.0");
            sheet1.SetContentsOfCell("1x", "hello");
            Assert.AreEqual("hello", sheet1.GetCellContents("1X"));
        }

        /// <summary>
        /// basic spreadsheet creation test, ensures that default normalizer does nothing to name
        /// </summary>
        [TestMethod]
        public void threeParamSpreadsheetTest3()
        {
            Spreadsheet sheet1 = new(s => true, s => s, "1.0");
            sheet1.SetContentsOfCell("x1", "1.0");
            Assert.AreEqual("", sheet1.GetCellContents("X1"));
            Assert.AreEqual(1.0, sheet1.GetCellContents("x1"));
        }
        /// <summary>
        /// provides false as the validator, should throw error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void threeParamSpreadsheetTest4()

        {
            Spreadsheet sheet1 = new(s => false, s => s, "1.0");
            sheet1.SetContentsOfCell("x1", "1.0");
        }

        /// basic spreadsheet creation test, ensures that default normalizer does nothing to name
        /// </summary>
        [TestMethod]
        public void getSavedVersionTest1()
        {
            Spreadsheet sheet1 = new(s => true, s => s, "1.0");
            sheet1.Save("test1.xml");
            Assert.AreEqual("1.0", sheet1.GetSavedVersion("test1.xml"));
        }

        //!This section contains four parameter tests
        /// <summary>
        /// Tests four parameter constructor loads a saved spreadsheet properly
        /// </summary>
        [TestMethod]
        public void fourParamSpreadsheetTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "10");
            sheet1.SetContentsOfCell("B1", "Hello world");
            sheet1.SetContentsOfCell("C1", "=A1 - 1");
            sheet1.Save("test3.xml");
            Spreadsheet sheet2 = new("test3.xml", s => true, s => s, "default");
            Assert.AreEqual(10.0, sheet2.GetCellContents("A1"));
            Assert.AreEqual("Hello world", sheet2.GetCellContents("B1"));
            Assert.AreEqual(new Formula("A1-1"), sheet2.GetCellContents("C1"));
            Assert.AreEqual(10.0, sheet2.GetCellValue("A1"));
            Assert.AreEqual("Hello world", sheet2.GetCellValue("B1"));
            Assert.AreEqual(9.0, sheet2.GetCellValue("C1"));
        }

        /// <summary>
        /// Tests an invalid filepath on four param spreadsheet
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourParamSpreadsheetTest2()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "10");
            sheet1.SetContentsOfCell("B1", "Hello world");
            sheet1.SetContentsOfCell("C1", "=A1 - 1");
            sheet1.Save("test3.xml");
            Spreadsheet sheet2 = new("3test.xml", s => true, s => s, "default");
        }

        /// <summary>
        /// Tests mismatching spreadsheet versioning
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourParamSpreadsheetTest3()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "10");
            sheet1.SetContentsOfCell("B1", "Hello world");
            sheet1.SetContentsOfCell("C1", "=A1 - 1");
            sheet1.Save("test4.xml");
            Spreadsheet sheet2 = new("test4.xml", s => true, s => s, "1.0");
        }

        /// <summary>
        /// Tests loading an invalid xml file, missing a cell tag
        /// Note: it was easier to create a file and write in incorrect xml syntax than finding some way to make my save write incorrectly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourParamSpreadsheetTest4()
        {
            File.WriteAllText("test8", "<?xml version=\"1.0\" encoding=\"utf-8\"?><spreadsheet version=\"default\"><name>A</name><contents>10</contents></cell><cell><name>B</name><contents>Hello world</contents></cell><cell><name>C</name><contents>=A-1</contents></cell></spreadsheet>");
            Spreadsheet sheet2 = new("test8", s => true, s => s, "default");
        }

        /// <summary>
        /// Tests invalid spreadsheet versioning in xml, versions are different
        /// Note: it was easier to create a file and write in incorrect xml syntax than finding some way to make my save write incorrectly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void invalidVersioningTest1()
        {
            File.WriteAllText("test9", "<?xml version=\"1.0\" encoding=\"utf-8\"?><spreadsheet version=\"default\"><cell><name>A</name><contents>10</contents></cell><cell><name>B</name><contents>Hello world</contents></cell><cell><name>C</name><contents>=A-1</contents></cell></spreadsheet>");
            Spreadsheet sheet2 = new("test9", s => true, s => s, "1.0");
        }

        /// <summary>
        /// Tests missing version attribute
        /// Note: it was easier to create a file and write in incorrect xml syntax than finding some way to make my save write incorrectly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void invalidVersioningTest2()
        {
            File.WriteAllText("test10", "<?xml version=\"1.0\" encoding=\"utf-8\"?><spreadsheet><cell><name>A</name><contents>10</contents></cell><cell><name>B</name><contents>Hello world</contents></cell><cell><name>C</name><contents>=A-1</contents></cell></spreadsheet>");
            Spreadsheet sheet2 = new("test10", s => true, s => s, "1.0");
        }

        /// <summary>
        /// Tests getCellContent using a invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentInvalidNameTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "123");
            sheet1.GetCellContents("1A");
        }

        /// <summary>
        /// Tests getCellContent using a invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentInvalidNameTest2()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "123");
            sheet1.GetCellContents("");
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3+3");
            Assert.AreEqual(6.0, sheet1.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueTest2()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3-3");
            Assert.AreEqual(0.0, sheet1.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueTest3()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3*3");
            Assert.AreEqual(9.0, sheet1.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueTest4()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3/3");
            Assert.AreEqual(1.0, sheet1.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3*3");
            sheet1.SetContentsOfCell("B1", "=A1*3");
            Assert.AreEqual(27.0, sheet1.GetCellValue("B1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest2()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3*3");
            sheet1.SetContentsOfCell("B1", "=A1*3");
            sheet1.SetContentsOfCell("C1", "=B1+3");
            Assert.AreEqual(30.0, sheet1.GetCellValue("C1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest3()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=3*3");
            sheet1.SetContentsOfCell("B1", "=A1*A1");
            sheet1.SetContentsOfCell("C1", "=B1+B1");
            Assert.AreEqual(162.0, sheet1.GetCellValue("C1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest4()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=(B1+8) * 2");
            sheet1.SetContentsOfCell("B1", "10");
            sheet1.SetContentsOfCell("C1", "=A1 + D1");
            sheet1.SetContentsOfCell("D1", "=B1 * 2");
            sheet1.SetContentsOfCell("E1", "=C1 / 2");
            Assert.AreEqual(28.0, sheet1.GetCellValue("E1"));
            sheet1.SetContentsOfCell("B1", "11");
            Assert.AreEqual(30.0, sheet1.GetCellValue("E1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the value of cells as opposed to the content
        /// This particular test proves that order formulas entered doesn't matter as A is dependent on B before B is set
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest5()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=B1+1");
            sheet1.SetContentsOfCell("B1", "=10+2");
            sheet1.SetContentsOfCell("C1", "=5+5");
            sheet1.SetContentsOfCell("D1", "=89-1");
            Assert.AreEqual(13.0, sheet1.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that getCellValue returns the FormulaError when evaluating a formula containing an non-existant cell
        /// </summary>
        [TestMethod]
        public void getCellValueFormulaTest6()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=B1+1");
            sheet1.SetContentsOfCell("B1", "=C1+2");

            Assert.IsTrue(sheet1.GetCellValue("B1") is FormulaError);
        }

        /// <summary>
        /// Tests that getxml is working properly
        /// </summary>
        [TestMethod]
        public void getXMLTest()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "10");
            sheet1.SetContentsOfCell("B1", "Hello world");
            sheet1.SetContentsOfCell("C1", "=A1 - 1");
            Assert.IsTrue(sheet1.GetXML() is string);
        }

        /// <summary>
        /// Tests that save is saving a document
        /// </summary>
        [TestMethod]
        public void saveTest1()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "10");
            sheet1.SetContentsOfCell("B1", "Hello world");
            sheet1.SetContentsOfCell("C1", "=A1 - 1");
            sheet1.Save("test2.xml");
            Assert.AreEqual("default", sheet1.GetSavedVersion("test2.xml"));
        }

        /// <summary>
        /// Tests empty spreadsheet get value
        /// </summary>
        [TestMethod]
        public void emptyGetCellValueTest()
        {
            Spreadsheet sheet1 = new();
            Assert.AreEqual("", sheet1.GetCellValue("B1"));
        }

        /// <summary>
        /// Tests abstract constructors
        /// </summary>
        [TestMethod]
        public void constructorTest() 
        {
            AbstractSpreadsheet sheet1 = new Spreadsheet();
            AbstractSpreadsheet sheet2 = new Spreadsheet(s => true, s => s, "version one");
            sheet2.SetContentsOfCell("A1", "one");
            sheet2.Save("abTest1");
            AbstractSpreadsheet sheet3 = new Spreadsheet("abTest1", s=>true, s=>s, "version one");
        }

        /// <summary>
        /// Tests what happens when drawing a writing error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void errorReadingTest()
        {
            Spreadsheet sheet1 = new("DummyNonexistantFile", s => true, s => s, "default");
        }


        /// <summary>
        /// Tests invalid formula passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidFormulaTest()
        {
            Spreadsheet sheet1 = new();
            sheet1.SetContentsOfCell("A1", "=X+-4");
        }
    }
}

/// <summary>
/// Authors: Araum Karimi, Sam Oblad
/// Date: 3/3/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// We, Araum Karimi and Sam Oblad, certify that we wrote this code from scratch and
/// did not copy it in part or whole from another source. All
/// references used in the completion of the assignments are cited 
/// in the README file.
/// 
/// File Contents
/// This file contains my test cases for my spreadsheet class
/// </summary>
using SS;
namespace SpreadsheetUtilities;

/// <summary>
/// Tester/Demo for ChangesTracker
/// </summary>
[TestClass]
public class ChangesTester
{
    /// <summary>
    /// Tests adding to tracker through new change method
    /// </summary>
    [TestMethod()]
    public void TestNewChange()
    {
        ChangesTracker t = new ChangesTracker();

        t.NewChange(new Change("entry1", "", "content1"));

        t.NewChange(new Change("entry2", "", "content2"));

        t.NewChange(new Change("entry3", "", "content3"));
        
    }

    /// <summary>
    /// Tests undo and redo functionality
    /// </summary>
    [TestMethod()]
    public void TestGoBack()
    {
        ChangesTracker t = new ChangesTracker();
        Spreadsheet ss = new Spreadsheet();

        string oldContent = (string)ss.GetCellContents("entry1");
        ss.SetContentsOfCell("entry1", "content1");
        t.NewChange(new Change("entry1", oldContent, (string)ss.GetCellContents("entry1")));

        Change undo = t.GoBack();
        ss.SetContentsOfCell(undo.Name, undo.CachedContent);

        Change redo = t.GoForward();
        ss.SetContentsOfCell(redo.Name, redo.CachedContent);
    }

    /// <summary>
    /// Tests go Forward method in addition to undo redo functionality
    /// </summary>
    [TestMethod()]
    public void TestGoForward()
    {

        ChangesTracker t = new ChangesTracker();
        Spreadsheet ss = new Spreadsheet();

        // Store the change BEFORE setting in spreadsheet
        t.NewChange(new Change("entry1", (string)ss.GetCellContents("entry1"), "content1"));
        ss.SetContentsOfCell("entry1", "content1");
        Assert.AreEqual("content1", ss.GetCellContents("entry1"));

        Change undo1 = t.GoBack();
        ss.SetContentsOfCell(undo1.Name, undo1.CachedContent);
        Assert.AreEqual("", ss.GetCellContents("entry1"));  

        t.NewChange(new Change("entry2", (string)ss.GetCellContents("entry2"), "content2"));
        ss.SetContentsOfCell("entry2", "content2");
        Assert.AreEqual("content2", ss.GetCellContents("entry2"));

        t.NewChange(new Change("entry2", (string)ss.GetCellContents("entry2"), "content3"));
        ss.SetContentsOfCell("entry2", "content3");
        Assert.AreEqual("content3", ss.GetCellContents("entry2"));

        Change undo2 = t.GoBack();
        ss.SetContentsOfCell(undo2.Name, undo2.CachedContent);
        Assert.AreEqual("content2", ss.GetCellContents("entry2"));

        Change undo3 = t.GoBack();
        ss.SetContentsOfCell(undo3.Name, undo3.CachedContent);
        Assert.AreEqual("", ss.GetCellContents("entry2"));

        Change redo1 = t.GoForward();
        ss.SetContentsOfCell(redo1.Name, redo1.CachedContent);
        Assert.AreEqual("content2", ss.GetCellContents("entry2"));

        Change redo2 = t.GoForward();
        ss.SetContentsOfCell(redo2.Name, redo2.CachedContent);
        Assert.AreEqual("content3", ss.GetCellContents("entry2"));

    }

    /// <summary>
    /// Stress tests to assure that stacks are updating correctly
    /// </summary>
    [TestMethod()]
    public void StressTest()
    {
        ChangesTracker t = new ChangesTracker();
        Spreadsheet ss = new Spreadsheet();
        int numberOfChanges = 1000; // Define a large number of changes for the stress test

        // Apply a large number of changes
        for (int i = 0; i < numberOfChanges; i++)
        {
            string cellName = "cell" + i;
            string content = "content" + i;
            t.NewChange(new Change(cellName, (string)ss.GetCellContents(cellName), content));
            ss.SetContentsOfCell(cellName, content);
            Assert.AreEqual(content, ss.GetCellContents(cellName));
        }

        // Undo all changes
        for (int i = numberOfChanges - 1; i >= 0; i--)
        {
            Change undoChange = t.GoBack();
            ss.SetContentsOfCell(undoChange.Name, undoChange.CachedContent);
            Assert.AreEqual("", ss.GetCellContents(undoChange.Name)); 
        }

        // Redo all changes
        for (int i = 0; i < numberOfChanges; i++)
        {
            Change redoChange = t.GoForward();
            ss.SetContentsOfCell(redoChange.Name, redoChange.CachedContent);
            string expectedContent = "content" + i;
            Assert.AreEqual(expectedContent, ss.GetCellContents(redoChange.Name));
        }
    }

    

}

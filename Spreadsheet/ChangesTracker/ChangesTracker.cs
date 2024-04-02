/// <summary>
/// Author: Araum Karimi
/// Partner: Sam Oblad
/// Date: 3/03/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// We, Araum Karimi and Sam Oblad, certify that we wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in the README file.
/// </summary>
namespace SpreadsheetUtilities;

/// <summary>
/// ChangesTracker is a class that tracks changes made to cells in a spreadsheet. This class uses stacks to track
/// changes, with the ability to go forwards and backwards through the changes.
/// 
/// </summary>
public class ChangesTracker
{
    /// <summary>
    /// Represents a stack of changes elements going backwards
    /// </summary>
    private Stack<Change> BackwardStack;

    /// <summary>
    /// Represents a stack of changes elements going forwareds
    /// </summary>
    private Stack<Change> ForwardStack;

    /// <summary>
    /// Constructor for creating a new tracker
    /// </summary>
    public ChangesTracker()
    {
        BackwardStack = new();
        ForwardStack = new();
    }

    /// <summary>
    /// Determines if there are any possible changes to revert backward to
    /// </summary>
    /// <returns> True if there are backward changes available, false if not </returns>
    public bool CanGoBack()
    {
        return BackwardStack.Count() > 0;
    }

    /// <summary>
    /// Determines if there are any possible changes to revert forward to
    /// </summary>
    /// <returns> True if there are forward changes available, false if not </returns>
    public bool CanGoForward()
    {
        return ForwardStack.Count() > 0;
    }

    /// <summary>
    /// Resets the tracker, so that there are no backward or forward changes
    /// </summary>
    public void Reset()
    {
        BackwardStack = new();
        ForwardStack = new();
    }

    /// <summary>
    /// Adds a new change to the tracker's history. This method should be called whenever a cell's value is set
    /// </summary>
    /// <param name="element"> Change that was made </param>
    public void NewChange(Change element)
    {
        BackwardStack.Push(element);
        ForwardStack.Clear();
    }

    /// <summary>
    /// Adds a new change to the tracker's history. This method should be called whenever a cell's value is set
    /// </summary>
    /// <param name="element"> Change that was made </param>
    public void NewChange()
    {
        ForwardStack.Clear();
        BackwardStack.Pop();
    }

    /// <summary>
    /// Peeks the last change that was recorded in this tracker, and returns it
    /// </summary>
    /// <returns> Last change made </returns>
    /// <exception cref="Exception"> If no changes have been made</exception>
    public Change PeekLastChange()
    {
        if (ForwardStack.Count > 0)
        {
            return ForwardStack.Peek();
        }
        if (BackwardStack.Count == 0)
        {
            throw new Exception("No changes have been made");
        }
        return BackwardStack.Peek();
    }

    /// <summary>
    /// Goes back to the last change that was made, and returns it.
    /// </summary>
    /// <returns> The last change that was made </returns>
    /// <exception cref="Exception"> If there are no backward changes </exception>
    public Change GoBack()
    {
        if (BackwardStack.Count == 0)
        {
            throw new Exception("No backward changes to revert");
        }
        Change oldState = BackwardStack.Pop();
        Change newState = new Change(oldState.Name, oldState.NewContent, oldState.CachedContent);
        ForwardStack.Push(newState);
        return oldState;
    }

    /// <summary>
    /// Goes forward to the last change that was made, and returns it.
    /// </summary>
    /// <returns> The last change that was made </returns>		NewContent	"hello"	string

    /// <exception cref="Exception"> If there are no forward changes </exception>
    public Change GoForward()
    {
        if (ForwardStack.Count == 0)
        {
            throw new Exception("No forward changes to revert");
        }
        Change oldState = ForwardStack.Pop();
        Change newState = new Change(oldState.Name, oldState.NewContent, oldState.CachedContent);
        BackwardStack.Push(newState);
        return newState;
    }
}

/// <summary>
/// This class serves as storage for each change.
/// An instance of Change stores:
/// - Name of cell
/// - Content before change
/// - Content after change
/// </summary>
public class Change
{
    public string Name { get; set; }
    public string CachedContent { get; set; }
    public string NewContent { get; set; }

    /// <summary>
    /// Constructor for creating an instance of a new Change
    /// </summary>
    /// <param name="name"> Cell name </param>
    /// <param name="cachedContent"> Old contents of cell </param>
    /// <param name="newContent"> New contents after the change </param>
    public Change(string name, string cachedContent, string newContent)
    {
        Name = name;
        CachedContent = cachedContent;
        NewContent = newContent;
    }
}


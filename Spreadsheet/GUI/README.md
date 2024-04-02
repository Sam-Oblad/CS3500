Authors:		Sam Oblad
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		AraumKarimi, Sam-Oblad
Repo:			https://github.com/uofu-cs3500-spring24/assignment-six-gui-functioning-spreadsheet-variable_vikings
Commit Date:	3/3/2024
Solution:		Spreadsheet
Copyright:		CS3500, Araum Karimi and Sam Oblad - This work may not be copied for use in Academic Coursework.


# Overview:
A gui representation of our spreadsheet solution. It starts with a 10x10 grid, and has three widgets displaying data of the currently selected cell. Our additional feature is the Undo and Redo buttons,
which undo/redo the last change made to the sheet. Spreadsheets can be saved and loaded safely, as unsaved data loss protections are in place. Setting a cell to an invalid formula will show an error, 
and creating circular dependencies will notify the user. There is also a help page that documents spreadsheet functionalities.


# Comments to evaluators:
Our spreadsheet is only 10x10 because it took a while to load a 26x99 grid, however, we use const int variables (row, column) in our mainpage that can easily
resize the spreadsheet to any desired dimensions. Spreadsheets will always save with an A1 cell set to an empty string. This should have no bad effects on saving/loading, but makes it easier
to identify if a spreadsheet has changed when loading and saving. 


# Working with partners:
Our partnership was most effective at pair programming. We were able to make a lot of headway each time we met. We utilized live share which simplified and made our pair programming
sessions more efficient. It seemed when one of us was lost in the abstraction of the assignment, the other was able to navigate well. 
 
One area our partnership struggled in was transferring code between our machines. Araum has been using a windows machine rented from the library, but it doesn't have the permissions to
run MAUI code, so all of the GUI testing had to be done on Sam's computer. Araum did some work on his mac, but transferring that code to Visual Studio on Sam's machine was a pain.
In the future Araum will try booting up windows on his mac with bootcamp.




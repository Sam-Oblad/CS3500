Authors:		Araum Karimi & Sam Oblad
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		AraumKarimi, Sam-Oblad
Repo:			https://github.com/uofu-cs3500-spring24/assignment-six-gui-functioning-spreadsheet-variable_vikings
Commit Date:	3/3/2024 
Project:		ChangesTester
Copyright:		CS3500, Araum Karimi and Sam Oblad - This work may not be copied for use in Academic Coursework.

# Overview
This project contains a custom class that tracks changes made to a spreadsheet. The ChangesTracker stores cell names, old values, and new values
in order to navigate through them later. Utilizes a custom 'Change' class, which are the objects that actually store names and values. Change ordering is tracked using 2 stacks, 
one for 'forward' changes and one for 'backward' changes. 

# Comments to evaluators
This Class was designed for a spreadsheet, but is not specific to them. It has a simple tester/demo project, but extensive testing was done using the GUI.
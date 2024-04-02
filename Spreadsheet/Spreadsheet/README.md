'''
Author:			Sam Oblad
Parter:			None
Start Date:		13-Feb-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		sam-oblad
Repo:			https://github.com/Sam-Oblad/Spreadsheet
Project:		Spreadsheet
Copyright:		CS3500 and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview
This project contains my spreadsheet class, it was implemented using AbstractSpreadsheet.cs as a guide.
It serves as a foundation for my spreadsheet, and will be used to implement a GUI in the future.

# Comments to evaluator
Originally, I thought the best way to deal with finding values of cells was lazy evaluation, looking up values only when needed. In class, Professor De St. Germain
said that we should just store the values as we put them in our spreadsheet, and utilize the GetCellsToRecalculate as a way to recalculate the cells that changed.
I have since updates this, and now my spreadsheet works as described by the professor. 

I thought that changed should be set to true when constructing a new spreadsheet was appropriate, that way you could still save a completely empty spreadsheet if 
you wanted to

I decided that it was simpler for me to understand if I just removed the old dependencies and added new ones if there were existing dependencies rather than 
using the replace method.
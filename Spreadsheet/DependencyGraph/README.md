'''
Author:			Sam Oblad
Parter:			None
Start Date:		20-Jan-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		sam-oblad
Repo:			https://github.com/Sam-Oblad/Spreadsheet
Project:		DependencyGraph
Copyright:		CS3500 and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview
My DependencyGraph consists of a HashSet<string s, string t> that contains the main graph. t is always dependent on s.
I Utilized two Dictionary<string, HashSet<string>> to keep track of dependents and dependees.
This project will be used by the spreadsheet to determine which cells should be evaluated first, before being used in subsequent formulas.


# Comments to evaluator

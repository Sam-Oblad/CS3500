'''
Author:			Sam Oblad
Parter:			None
Start Date:		28-Jan-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		sam-oblad
Repo:			https://github.com/Sam-Oblad/Spreadsheet
Project:		Formula
Copyright:		CS3500 and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview
This project includes my formula class, which is a refactoring of the formulaEvaluator class. This class is different in that before evaluating, we create an instance of the
formula class. Before the object is created, it goes through a series of tests to ensure syntactic accuracy, if not correct a FormulaFormatException is thrown with helpful dialog.
The Formula class consists of three string properties, all of which are read only so that a formula is immutable after construction. 

string infixExpression; This contains the original formula passed as an argument to the constructor
string normalizedExpression; This contains a processed and normalized version of the infix expression, this includes normalizing and validation using appropriate delegates
string hashString; This contains the normalized expression, but is necessary to store values such as 2.00000 and 2 being identical for proper hashing


# Comments to evaluator

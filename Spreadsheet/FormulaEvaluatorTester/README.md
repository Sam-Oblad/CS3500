'''
Author:			Sam Oblad
Parter:			None
Start Date:		13-Jan-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		sam-oblad
Repo:			https://github.com/Sam-Oblad/Spreadsheet
Project:		FormulaEvaluatorTester
Copyright:		CS3500 and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview
This project is a console project that is designed to test and evaluate the functionality of the FormulaEvaluator project. It tests addition, subtraction,
multiplication, and division. It also tests complex formulas combining all of these operations with the use of parentheses. This program also tests the FormulaEvaluator's
functionality of being passed variables within formulas.

# Comments to Evaluators
My test program tests variables by storing them in a dictionary, (string, int) where the string is the variable name and the int is the value the variable represents.
Then I wrote a delegate function that is passed into my Evaluator.Evaluate() method to look up the variable and return its value. Unit testing probably
would have made this program much neater, and my testing functions are probably unorthodox, but they work well and it's simple for me to add new tests or edit
existing ones. 
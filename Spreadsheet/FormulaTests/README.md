'''
Author:			Sam Oblad
Parter:			None
Start Date:		28-Jan-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		sam-oblad
Repo:			https://github.com/Sam-Oblad/Spreadsheet
Project:		FormulaTests
Copyright:		CS3500 and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview
This project contains all of the tests for my Formula class. Testing includes: simple arithmetic, complex parentheses, variables, validators, formula equalities, hashcode equalities,
the use of validators and normalizers, and a mixture of everything above. 

# Comments to evaluator
I drew inspiration for some of my tests from the given graded tests from assignment 1 since they are a 
way to make sure that my infix calculation algorithm was migrated over successfully.
I did, however, change all numbers, variables, and as a result, answers. I only used them
as a suggestion for exhaustive test cases. I added operations to each one, and I listed the repo as a reference. I believe this 
should be reasonable since I did rewrite every single test, and did not copy and paste. I also wrote ~65 of my own tests.

Note: After rereading instructions, posing a question on Piazza, and specifically seeing that we aren't allowed to use old tests,
I decided to remove everything resembling gradedTests for assignment1 from my testing suite because I don't want to be docked for unoriginality. Since we are refactoring 
code from assignment 1, it would be fair to assume that we could use old test cases to retest old code, since we've already received a grade on it and the
test cases that really matter are for methods specific to Assignment 3. However, I removed everything anyway. I trust that this will remedy my use of the 
old tests as guidelines, as my code coverage is at 97.5% even without the old unit tests from assignment1.

After removal, I wrote two additional tests, massiveFormulaEvaluationTest1 & 2. These brought my code coverage back to 99%. Everything should be visible in 
my versioning logs, and I left the gradedAnswer repo in as a reference, however, none of the code I'm turning in should look similar to the given gradedTest1 file. 


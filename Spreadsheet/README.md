'''
Authors:		Araum Karimi & Sam Oblad
Start Date:		13-Jan-2024
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		AraumKarimi, Sam-Oblad
Repo:			https://github.com/uofu-cs3500-spring24/assignment-six-gui-functioning-spreadsheet-variable_vikings
Commit Date:	3/3/2024 1400
Solution:		Spreadsheet
Copyright:		CS3500, Araum karimi and Sam Oblad - This work may not be copied for use in Academic Coursework.
'''

# Overview of the Spreadsheet functionality:

	The Spreadsheet program is currently four libraries, one containing my FormulaEvaluator, another containing my DependencyGraph, and
	a third containing the bones of my spreadsheet class. My fourth library is now a complete spreadsheet class to run my gui which is now in development.

# Time Expenditures:
	
	1. Assignment One: Predicted Hours:		12		Actual Hours:		11
	2. Assignment Two: Predicted Hours:		10		Actual Hours:		9.5
	3. Assignment Three: Predicted Hours:   12		Actual Hours:		14
	4. Assignment Four: Predicted Hours:    12      Actual Hours:       8
	5. Assignment Five: Predicted Hours:    9       Actual Hours:       15
	6. Assignment Six: Predicted Hours:     10      Actual Hours:       18



	3/3/2024:
	I think estimates have been improving, however this week we may have underestimated how long this gui would take. 

# Comments to Evaluators:

	I implemented an infix calculator that handles order of operations and parentheses. This program also can handle looking up a variable within a formula
	by passing a delegate. I tested my variables by putting them into a dictionary<string, int> where string is the name of the variable and int is the value
	associated with it. I think that this is the best way to do so as we discussed this in class. I found the delegate to be a little confusing, 
	but after implementing it, I realize that delegates are really cool as I only need to write the lookUp call within Evaluator, and if someone wanted to,
	they could have it iterate through an array to retrieve a value instead of using a dictionary. I believe my work stands on its own.

	My dependency graph currently passes all given tests as well as ~30 additional tests I wrote for each method I tried to break each method up and test it 
	thoroughly with different values, null values, sometimes no values. I also tested running multiple graphs at the same time. 

	My formula class is now implemented, tested, and working. This makes my formulaEvaluator project obsolete as it verifies syntax accuracy prior to formula construction,
	and can solve the formulas using the same infix calculator. 

	My Spreadsheet class is now implemented and tested. Code coverage is at 100%, and appropriate errors are thrown
	as defined in AbstractSpreadsheet.cs, additionally, all documentation is in AbstractSpreadsheet.cs, and was imported using xml tags
	to Spreadsheet.cs

	For our additional feature in our gui, we added undo and redo buttons. The code for these features is in the ChangesTracker project. 
	

# Examples of Good Software Practice:

	Within my spreadsheet class, my Save and GetXML files separate problems and solve one problem each. I chose to have my GetXML method to build a string resembling xml code for each non
	empty cell within my spreadsheet. When done it returns the entire string. My save method is the one that actually creates a file and writes to it using
	the XMLwriter by calling GetXML and using the writeRaw method. I found this was the easiest way to accomplish saving a file, and it works when reading as well.

	Again, within my spreadsheet class I found that writing a helper method to validate names for use in the spreadsheet was the easiest way to check if a name is
	valid. This helps me not repeat myself. Every time I check a name, I can just call the method and give it the name and it will tell me true or false. This is also an
	example of a very short method that does only one specific thing. This makes for easier maintainability when refactoring code. 

	When testing my spreadsheet class, I created multiple stress tests to ensure that stack overflows are not occurring. I actually was able to find a bug in my code that
	was leading to infinite recursion because of these stress tests. I also tried to add multiple tests on the same method but do different things. This is why my
	tests are often named test1 test2 test3 ... 


# More Examples of Good Software Practice:

	Instead of deleting all my old tests when refactoring my spreadsheet, I was able to retrofit them into the new api by changing a few of the methods. This helped me 
	save time by not rewriting tests that were already valid. This helped me to ensure that no new bugs have been introduced by the changes I've made to my
	spreadsheet class.

	In my spreadsheet class, I wrote a helper method titled ExtractVarsRemoveDependencies that is a very short method but it does a very specific job. It removes
	existing variables when updating a change to a formula cell. This is a good example of a helper method that doesn't do a bunch of different things. This helps
	in code maintainability.

	A good example of abstraction in my code is my implementation of GetDirectDependents in my spreadsheet class. This method is only one line, and it just returns
	the dependees of the named cell. This is a powerful method because it lets my dependency graph do all of the heavy lifting in storing dependencies. 



# Assignment Specific Topics:

# Consulted Peers:
	Brendan Beckstrom

# References

	https://stackoverflow.com/questions/141088/how-to-iterate-over-a-dictionary
	https://www.geeksforgeeks.org/c-sharp-count-the-total-number-of-elements-in-the-list/
	https://stackoverflow.com/questions/61022245/c-sharp-list-add-works-but-not-append
	https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/exception-handling
	https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
	https://learn.microsoft.com/en-us/dotnet/api/system.collections.stack?view=net-8.0
	https://www.geeksforgeeks.org/c-sharp-trim-method/
	https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
	https://www.tutorialsteacher.com/csharp/csharp-stack
	https://www.geeksforgeeks.org/stack-pop-method-in-c-sharp/
	https://www.geeksforgeeks.org/stack-peek-method-in-c-sharp/
	https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
	https://www.codecademy.com/resources/docs/c-sharp/strings/toupper
	https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder.-ctor?view=net-8.0#system-text-stringbuilder-ctor
	https://learn.microsoft.com/en-us/visualstudio/ide/reference/generate-equals-gethashcode-methods?view=vs-2022
	https://github.com/uofu-cs3500-spring24/ForStudents/blob/main/GradingTests/AssignmentOneGradingTests.cs
	https://www.w3schools.com/cs/cs_inheritance.php
	https://tunnelvisionlabs.github.io/SHFB/docs-master/SandcastleBuilder/html/79897974-ffc9-4b84-91a5-e50c66a0221d.htm
	https://stackoverflow.com/questions/5601777/constructor-of-an-abstract-class-in-c-sharp
	https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?view=net-8.0
	https://learn.microsoft.com/en-us/dotnet/api/system.io.file.create?view=net-8.0
	https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader?view=net-8.0
	https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/entry?view=net-maui-8.0
	https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows?view=net-maui-8.0
	https://learn.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-8.0
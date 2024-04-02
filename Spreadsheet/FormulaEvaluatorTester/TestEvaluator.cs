/// <summary>
/// Author: Sam Oblad
/// Partner: None
/// Date: 1/18/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// I, Sam Oblad, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
/// 
/// File Contents
/// My main function initiates a dictionary to contain all the incorrect answers received, and it counts how many are correct. Each individual test function adds incorrect tests
/// to this dictionary and at the end, main prints out the incorrect tests as well as let the user know if all tests have passed. 
/// After testing some basic formulas, it moves to test invalids and variables that have no value and are therefore invalid. It also tests a null delegate pass to Evaluate
/// And a divide by 0 case
/// 
/// </summary>
using FormulaEvaluator;

/// <summary>I wrote this function to test whether the trim() call was working </summary>
void testTrim()
{
    Console.WriteLine("Trim Test: ");
    Console.WriteLine(" 5+5 ");
    int response = Evaluator.Evaluate(" 5+5 ", null);
    Console.WriteLine($"5+5 = {response}");
}

/// <summary>This function tests basic addition formulas as well as order of operations</summary>
/// <param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runAddition(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    List<string> formulas = ["10", "5+5", " 5 + 20 ", "5 + 10 / 2"];
    List<int> answers = [10, 10, 25, 10];
    int response;

    for (int i = 0; i < formulas.Count; i++)
    {
        response = Evaluator.Evaluate(formulas[i], null);
        checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
    }

}

///<summary>This function tests basic subtraction formulas as well as order of operations</summary>
/// <param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runSubtraction(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    List<string> formulas = ["5-5", " 5 - 5 ", "5 - 10 / 2", "(2-4)*2"];
    List<int> answers = [0, 0, 0, -4];
    int response;

    for (int i = 0; i < formulas.Count; i++)
    {
        response = Evaluator.Evaluate(formulas[i], null);
        checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
    }
}

///<summary>This function tests basic multiplication formulas as well as order of operations</summary>
///<param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runMultiplication(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    List<string> formulas = ["5*5", " 5 * 5 ", "5*20/10"];
    List<int> answers = [25, 25, 10];
    int response;

    for (int i = 0; i < formulas.Count; i++)
    {
        response = Evaluator.Evaluate(formulas[i], null);
        checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
    }
}

///<summary>This function tests basic division formulas as well as order of operations and divide by 0</summary>
///<param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runDivision(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    List<string> formulas = ["5/5", " 5 / 5 ", "5/5*20", "5/0"];
    List<int> answers = [1, 1, 20];
    int response;

    try 
    {
        for (int i = 0; i < formulas.Count; i++)
        {
            response = Evaluator.Evaluate(formulas[i], null);
            checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
        }
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Cannot Divide by 0");
    }

}

///<summary>this function tests advanced formulas for proper order of operation</summary>
///<param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runComplex(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    List<string> formulas = ["(32/4)*(6+2)-9", "(15+7)*3-42", "(28-9)/3+(6*4)", "(10+5)*(9-2)", " ( 36/ 4)* (5+ 2) ", " ( ( ( 750 /5)- 10)/5) "];
    List<int> answers = [55, 24, 30, 105, 63, 28];
    for (int i = 0; i < formulas.Count; i++)
    {
        int response = Evaluator.Evaluate(formulas[i], null);
        checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
    }
}
///<summary>This function tests my own variables using a dictionary storing the (string name, int value) of the variables
///I also wrote a function that is passed as a delegate to evaluate in order to lookup and return the value of the variable by checking its name
/// </summary>
/// <param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
void runVariables(ref int correct, ref Dictionary<string, int>wrongAnswers)
{
    Console.WriteLine("Running test using variables: ");
    Dictionary<string, int> vars = new();
    vars.Add("a1", 5);
    vars.Add("Aa11", 50);
    vars.Add("aaa111", 100);
    vars.Add("x1", 20);
    vars.Add("abc123", 200);
    List<string> formulas = ["a1 + a1", "Aa11 - a1", "aaa111 * 2", " x1 / a1 ", "10 + a1", "(a1+(Aa11*10/(10-5)))"];
    List<int> answers = [10, 45, 200, 4, 15, 105];

    ///<summary>This is my delegate the looks up the value of my variable</summary>
    ///<param name="variable"> a string that represents a variable name</param>
    ///<returns>an int (kvp.Value) if value found, and throws an argument error if no value found</returns>
    int findValue(string variable)
    {
        foreach(KeyValuePair <string, int> kvp in vars)
        {
            if (variable == kvp.Key)
            {
                return kvp.Value;
            }
        }
        throw new ArgumentException();
    }

    for (int i = 0; i < formulas.Count; i++)
    {
        int response = Evaluator.Evaluate(formulas[i], findValue);
        checkWork(ref correct, ref wrongAnswers, answers[i], response, formulas[i]);
    }
}

///<summary>This function tests checking variables that have no value by using the findValue method</summary>
void runVariableInvalids()
{
    List<string> formulas = ["a1 + a1", "aa11 - a1", "aaa111 * 2", " x1 / a1 ", "10 + a1", "(a1+(aa11*10/(10-5)))"];
    List<string> nullVariablesFormulas = ["b1 + 1", "z8", "a1 / h6", " aaa11 + 1"];
    List<int> answers = [10, 45, 200, 4, 15, 105];
    int errorsCaught = 0;
    Dictionary<string, int> vars = new();
    vars.Add("a1", 5);
    vars.Add("aa11", 50);
    vars.Add("aaa111", 100);
    vars.Add("x1", 20);
    vars.Add("abc123", 200);

    ///<summary>This is my delegate the looks up the value of my variable</summary>
    ///<param name="variable"> a string that represents a variable name</param>
    ///<returns>an int (kvp.Value) if value found, and throws an argument error if no value found</returns>
    int findValue(string variable)
    {
        foreach (KeyValuePair<string, int> kvp in vars)
        {
            if (variable == kvp.Key)
            {
                return kvp.Value;
            }
        }
        throw new ArgumentException();
    }

    foreach (string formula in nullVariablesFormulas)
    {
        try
        {
            int response = Evaluator.Evaluate(formula, findValue);
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Syntax Error: \'{formula}\' is not a valid formula");
            errorsCaught++;
        }
    }
    if (errorsCaught == nullVariablesFormulas.Count())
    {
        Console.WriteLine($"All {errorsCaught} errors caught");
    }

    try
    {
        int testNullDelegate = Evaluator.Evaluate("a1 + a1", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Null provided as delegate, please provide non-null delegate");
    }
}

///<summary>This function tests all sorts of invalid formulas</summary>
void runInvalids()
{
    List<string> formulas = [null, "(8 + 7", "5 + ", "-10", "a1 +5", "(((6+5)+4)", "8+7)" , "()", "( )", "(", "", "1a", "a", "((5 - 0)", "1-4(", "A1", " ", "?", "10 + + 10", "10 + - 10", "+ 9"];
    int count = 0;
    for (int i = 0; i < formulas.Count; i++)
    {
        try
        {
            Evaluator.Evaluate(formulas[i], null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Syntax Error: \'{formulas[i]}\' is not a valid formula");
            count++;
        }
    }
    if (count == formulas.Count)
    {
        Console.WriteLine($"All {count} basic formulas successfully caught as invalid");
        Console.WriteLine();
    }
    
}

///<summary>This function is more of a helper function and just handles incorrect responses from the above tests and stores them as either a increment on correct, or puts the formula
///into my wrongAnswer dictionary so it can be printed out later
/// </summary>
/// <param name="correct"> int that tracks correct formulas processed</param>
/// <param name="wrongAnswers"> Dictionary containing incorrectly process formula and the answer given</param>
/// <param name="response"> an int containing the incorrect answer from Evaluator.Evaluate</param> 
/// <param name="formula">The formula that was not evaluated correctly</param>
void checkWork(ref int correct, ref Dictionary<string, int> wrongAnswers, int correctAns, int response, string formula)
{
    if (response == correctAns)
    {
        correct++;
    }
    else
    {
        wrongAnswers.Add(formula, response);
    }
}

///<summary>This function calls all the other test functions</summary>
void runTests(ref int correct, ref Dictionary<string, int> wrongAnswers)
{
    //testTrim();
    runAddition(ref correct, ref wrongAnswers);
    runSubtraction(ref correct, ref wrongAnswers);
    runMultiplication(ref correct, ref wrongAnswers);
    runDivision(ref correct, ref wrongAnswers);
    runComplex(ref correct, ref wrongAnswers);
    runVariables(ref correct, ref wrongAnswers);
}

///<summary>
///My main function sets everything up and calls runTests, runInvalids, and runVariableInvalids
///I manually count of all the basic tests (runAddition, subtraction, multiplication, division, complex, and variables) this excludes both invalid test funcs
/// </summary>
///<returns>returns 0</returns>
int main()
{
    int correct = 0;
    Dictionary<string, int> wrongAnswers = [];
    runTests(ref correct, ref wrongAnswers);
    Console.WriteLine($"Answers Correct: {correct}");
    if (correct == 26)
    {
        Console.WriteLine("All basic tests passed");
        Console.WriteLine();
    }

    if (wrongAnswers.Count > 0)
    {
        Console.WriteLine();
        Console.WriteLine($"There were {wrongAnswers.Count} incorrect answers to the following formulas: ");
        foreach (KeyValuePair<string, int> entry in wrongAnswers)
        {
            Console.WriteLine($"{entry.Key} = {entry.Value}");
        }
    }

    Console.WriteLine("Testing Invalids: ");
    runInvalids();

    Console.WriteLine("Testing Invalid variable formulas:");
    runVariableInvalids();

 

    return 0;
}

main();
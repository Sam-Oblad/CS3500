// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
//
//// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <author> Araum Karimi </author>
    /// <version> February 6, 2024 </version>
    /// 
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private readonly List<string> formulaTokens = new();
        private readonly HashSet<string> formulaVariables = new();


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(string formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            try
            {
                ValidateSyntax(formula, normalize, isValid);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new();
            Stack<string> operatorStack = new();

            // Iterate through each token, and manipulate the stacks corresponding to its value.
            // Notice "+" and "-" share a case, "*", "/", and "(" share a case, variables and integers somewhat share a case.
            // If a token is not valid, an Argument Exception is thrown.
            foreach (var t in formulaTokens)
            {

                double potentialVariable = 0;
                // token is an operator, including parenthesis
                if (Extensions.Extensions.IsOperator(t))
                {
                    try
                    {
                        EvaluateOperatorHelper(valueStack, operatorStack, t);
                    }
                    catch (ArgumentException) { return new FormulaError("Divide by zero error"); }
                }
                // Check if token is variable or integer. Either way, do the same thing just on a different number
                else if (!Extensions.Extensions.IsOperator(t))
                {
                    // If variable, call lookup its value, otherwise continue using token
                    if (Extensions.Extensions.IsVariable(t))
                    {
                        try
                        {
                            potentialVariable = lookup(t);
                        }
                        catch (Exception) { return new FormulaError("Variable " + potentialVariable + " not found"); }
                    }
                    else if (Extensions.Extensions.IsNumber(t))
                    {
                        potentialVariable = double.Parse(t);
                    }
                    if (Extensions.Extensions.IsNumber(t) || Extensions.Extensions.IsVariable(t))
                    {
                        try
                        {
                            EvaluateNumberHelper(valueStack, operatorStack, potentialVariable);
                        }
                        catch (Exception) { return new FormulaError("Divide by zero error"); }
                    }
                }
            }
            // If operatorStack is empty after iterating
            if (operatorStack.Count() == 0 && valueStack.Count() == 1)
            {
                return valueStack.Pop();
            }
            // If operatorStack is not empty and valueStack has two items after iteration
            else if (operatorStack.Count() == 1 && valueStack.Count() == 2)
            {
                if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
                {
                    EvaluateAddSubtractHelper(valueStack, operatorStack);
                }
            }
            return valueStack.Pop();
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return formulaVariables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var token in formulaTokens)
            {
                sb.Append(token);
            }
            return sb.ToString();
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Formula)
            {
                Formula formula = (Formula)obj;
                List<string> comparisonTokens = formula.formulaTokens;
                if (formulaTokens.Count() == comparisonTokens.Count())
                {
                    for (int i = 0; i < formulaTokens.Count(); i++)
                    {
                        if (!formulaTokens[i].Equals(comparisonTokens[i])) { return false; }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int result = 0;
            for (int i = 0; i < formulaTokens.Count(); i++)
            {
                result += formulaTokens[i].GetHashCode() + 7 * i;
            }
            return result;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall expPattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Helper method that validates if the formula passed in is syntactically correct. 
        /// Allows the user to pass in custom normalize and variable validator functions.
        /// Used in constructor so that errors can be caught at object initialization.
        /// Side effect: Adds variables to formulaTokens and formulaVariables, because it is used in the constructor
        /// </summary>
        /// <exception cref="FormulaFormatException"> If formula is syntactically incorrect </exception>
        private void ValidateSyntax(string formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            List<string> tokenList = GetTokens(formula).ToList();
            int parenthesisCount = 0;

            // Check for at least 1 token
            if (tokenList.Count < 1) { throw new FormulaFormatException("Invalid syntax: empty string"); }

            // Iterate through tokens
            for (int i = 0; i < tokenList.Count(); i++)
            {
                var currentToken = normalize(tokenList[i]);

                // Case token is an operator
                if (Extensions.Extensions.IsOperator(currentToken))
                {
                    switch (currentToken)
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            // Validate
                            if (!ValidateOperatorAdjacentTokens(tokenList, i))
                            {
                                throw new FormulaFormatException("Invalid syntax: Extra operator '" + currentToken + "'");
                            }
                            formulaTokens.Add(currentToken);
                            break;
                        case "(":

                            // Validate
                            if (!ValidateParenthesisAdjacentTokens(tokenList, i))
                            {
                                throw new FormulaFormatException("Invalid syntax: Missing operator between terms");
                            }
                            formulaTokens.Add(currentToken);
                            parenthesisCount++;
                            break;
                        case ")":
                            if (parenthesisCount <= 0)
                            {
                                throw new FormulaFormatException("Invalid syntax: Extra closing parenthesis");
                            }
                            formulaTokens.Add(currentToken);
                            parenthesisCount--;
                            break;
                    }
                }

                // Case token is a number or variable
                else if (Extensions.Extensions.IsNumber(currentToken) || Extensions.Extensions.IsVariable(currentToken))
                {
                    // Validate
                    if (!ValidateNumberAdjacentTokens(tokenList, i))
                    {
                        throw new FormulaFormatException("Invalid syntax: Missing operator between terms");
                    }

                    if (Extensions.Extensions.IsNumber(currentToken))
                    {
                        double.TryParse(currentToken, out double result);
                        formulaTokens.Add(result.ToString());
                    }
                    else if (Extensions.Extensions.IsVariable(currentToken))
                    {
                        if (isValid(currentToken))
                        {
                            // Store variable
                            formulaVariables.Add(normalize(currentToken));
                            formulaTokens.Add(normalize(currentToken));
                        }
                        else { throw new FormulaFormatException("Invalid Variable: '" + currentToken + "' improperly formatted"); }
                    }
                }

                else { throw new FormulaFormatException("Invalid syntax: Term '" + currentToken + "' is invalid"); }
            }
            if (parenthesisCount > 0) { throw new FormulaFormatException("Invalid syntax: Extra open parenthesis"); }
        }

        /// <summary>
        /// Validates that the elements adjacent to an operator (+,-,*,/) are syntactically correct 
        /// </summary>
        /// <param name="tokenList"> List of tokens </param>
        /// <param name="index"> Index of operator in tokenList </param>
        /// <returns> True if valid, false if invalid </returns>
        private bool ValidateOperatorAdjacentTokens(List<string> tokenList, int index)
        {
            if (index == 0 || index == tokenList.Count() - 1) { return false; }
            else
            {
                return (Extensions.Extensions.IsNumber(tokenList[index + 1]) || Extensions.Extensions.IsVariable(tokenList[index + 1]) || tokenList[index + 1] == "(") &&
                        (Extensions.Extensions.IsNumber(tokenList[index - 1]) || Extensions.Extensions.IsVariable(tokenList[index - 1]) || tokenList[index - 1] == ")");
            }
        }

        /// <summary>
        /// Helper method for ValidateSyntax that determines if the elements adjacent to a number (integer, variable, exponential) are syntactically correct 
        /// </summary>
        /// <param name="tokenList"> List of tokens </param>
        /// <param name="index"> Index of number in tokenList </param>
        /// <returns> True if valid, false if invalid </returns>
        private bool ValidateNumberAdjacentTokens(List<string> tokenList, int index)
        {

            if (index == 0)
            {
                if (tokenList.Count() == 1)
                {
                    return true;
                }
                return !(Extensions.Extensions.IsNumber(tokenList[index + 1]) || Extensions.Extensions.IsVariable(tokenList[index + 1]) || tokenList[index + 1] == "(");
            }
            else if (index == tokenList.Count() - 1)
            {
                return !Extensions.Extensions.IsNumber(tokenList[index - 1]) && !Extensions.Extensions.IsVariable(tokenList[index - 1]) && tokenList[index - 1] != ")";
            }
            return !(Extensions.Extensions.IsNumber(tokenList[index + 1]) || Extensions.Extensions.IsVariable(tokenList[index + 1]) || tokenList[index + 1] == "(" ||
                    Extensions.Extensions.IsNumber(tokenList[index - 1]) || Extensions.Extensions.IsVariable(tokenList[index - 1]) || tokenList[index - 1] == ")");
        }

        /// <summary>
        /// Helper method for ValidateSyntax that determines if the elements adjacent to an open parenthesis are syntactically correct
        /// </summary>
        /// <param name="tokenList"> List of tokens </param>
        /// <param name="index"> Index of number in tokenList </param>
        /// <returns> True if valid, false if invalid </returns>
        private bool ValidateParenthesisAdjacentTokens(List<string> tokenList, int index)
        {
            if (index == tokenList.Count() - 1) { return false; }

            if (index == 0) { return Extensions.Extensions.IsNumber(tokenList[index + 1]) || Extensions.Extensions.IsVariable(tokenList[index + 1]) || tokenList[index + 1] == "("; }

            else return (Extensions.Extensions.IsNumber(tokenList[index + 1]) || Extensions.Extensions.IsVariable(tokenList[index + 1]) || tokenList[index + 1] == "(");
        }

        /// <summary>
        /// Helper method that handles cases where the token passed into the Evaluate method is a number
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <param name="potentialVariable"> Note: In the context of the formuala, this could be the value of a variable or exponential </param>
        private static void EvaluateNumberHelper(Stack<double> valueStack, Stack<string> operatorStack, double numerical)
        {
            valueStack.Push(numerical);

            if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                EvaluateMultiplyDivideHelper(valueStack, operatorStack);
            }
        }

        /// <summary>
        /// Helper method that handles cases where the token passed into Evaluate method is an operator. Notice "(", "*", "/" share case,
        /// "+", "-" share a case, and ")" has its own case.
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <param name="token"></param>
        /// <exception cref="ArgumentException"> If divide by zero </exception>
        private static void EvaluateOperatorHelper(Stack<double> valueStack, Stack<string> operatorStack, string token)
        {
            switch (token)
            {
                // token is )
                case ")":
                    EvaluateRightParenthesisHelper(valueStack, operatorStack);
                    break;
                // token is (, *, or /
                case "(":
                case "*":
                case "/":
                    operatorStack.Push(token);
                    break;
                // token is + or -
                case "+":
                case "-":
                    if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(token);
                        break;
                    }
                    EvaluateAddSubtractHelper(valueStack, operatorStack);
                    operatorStack.Push(token);
                    break;
            }
        }

        /// <summary>
        /// Helper method that handles the right parenthesis case in the Evaluate method, handling value and operator stack operations 
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void EvaluateRightParenthesisHelper(Stack<double> valueStack, Stack<string> operatorStack)
        {
            if (operatorStack.Count() > 0)
            {
                if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
                {
                    EvaluateAddSubtractHelper(valueStack, operatorStack);
                }
            }
            if (operatorStack.Peek() == "(")
            {
                operatorStack.Pop();
            }
            if (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "*" || operatorStack.Peek() == "/")
                {
                    EvaluateMultiplyDivideHelper(valueStack, operatorStack);
                }
            }
        }

        /// <summary>
        /// Helper method that handles the addition and subtraction operations of the value and operator stacks in Evaluate method
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        private static void EvaluateAddSubtractHelper(Stack<double> valueStack, Stack<string> operatorStack)
        {
            if (operatorStack.Peek() == "+")
            {
                if (valueStack.Count() >= 2)
                {
                    valueStack.Push(valueStack.Pop() + valueStack.Pop());
                    operatorStack.Pop();
                }
            }
            else if (operatorStack.Peek() == "-")
            {
                if (valueStack.Count() >= 2)
                {
                    var temp = valueStack.Pop();
                    operatorStack.Pop();
                    valueStack.Push(valueStack.Pop() - temp);
                }
            }
        }

        /// <summary>
        /// Helper method that handles the multiplication and division operations of the value and operator stacks in Evaluate method
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <exception cref="ArgumentException"> If divide by zero </exception>
        private static void EvaluateMultiplyDivideHelper(Stack<double> valueStack, Stack<string> operatorStack)
        {
            if (operatorStack.Peek() == "*")
            {
                if (valueStack.Count >= 2)
                {
                    valueStack.Push(valueStack.Pop() * valueStack.Pop());
                    operatorStack.Pop();
                }
            }
            else if (operatorStack.Peek() == "/")
            {
                if (valueStack.Count >= 2)
                {
                    var temp = valueStack.Pop();
                    if (temp != 0)
                    {
                        valueStack.Push(valueStack.Pop() / temp);
                        operatorStack.Pop();
                    }
                    else { throw new ArgumentException("Divide by zero error"); }
                }
            }
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

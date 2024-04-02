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
/// This file contains my Evaluator class for my Dynamic Linked Library that will be used for evaluating formulas on a spreadsheet.
/// 
/// </summary>

using System.Text.RegularExpressions;
namespace FormulaEvaluator
{   /// <summary>
///  My evaluator class contains a delegate declaration and a Evaluate method used to evaluate a given formula (string) with a given delegate
/// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Delegate declaration, given a string to the delegate function, return the appropriate int
        /// </summary>
        /// <param name="variable_name"></param>
        /// <returns>the Int value found from the delegate</returns>
        public delegate int Lookup(String variable_name);

        /// <summary>
        /// My evaluate method takes a formula (string), and given a delegate, can retrieve variable values if necessary.
        /// Note: if no delegate is passed and variables are present an error is thrown, additionally, if no delegate is required, null can still be passed
        /// first, remove outside whitespace with trim, then declare value stack and operator stacks.
        /// Then, use the given regex formula to identify all tokens, then enter the main algorithm loom.
        /// 
        ///             /// Main Algorithm loop:
        /// 1)  check if the token is a valid integer using tryParse
        ///     Loop through tokens one at a time to process, if whitespace: ignore and proceed to next token, then Parse token
        ///     if Token is a valid integer call checkMD function 
        /// 
        /// 2)  check if token is a variable, first by checking the first and last chars that the first is a letter, and the last is a number
        ///     I check for a correct variable name by looking at the first and last char, which could leave room for some errors, 
        ///     but works if assuming no edge cases: "AAA1AA!?11, However, if the delegate does not return an int, it will throw an argument error anyway
        ///     When the delegate returns a proper int, we proceed the same as above by calling the checkMD function.
        ///     catch argumentException: Catches a call of lookup on token but no value is found
        ///     catch nullReferenceException: This catches a correct formula with a correct variable that does not receive a lookup delegate
        ///     catch IndexOutOfRangeException: This catches is necessary if an empty string: "" gets passed as a token
        /// 
        /// 3)  Next we check the token if it is a + or -, if it is: 
        ///     check op stack top for + or - and value stack has 2 or more elements, pop and carry out op, then put ans on value stack
        ///     first check the appropriate sizes of the stacks, then call valueCalc function
        ///     catch InvalidOperationException: This catches an empty operation stack error
        /// 
        /// 4)  Check if token is * or /, push onto op stack
        /// 
        /// 5)  Check if token is ), if it is, check op stack for + or - on top, pop, carry out op, put on value stack by calling valueCalc
        ///     Remove appropriate ( if on top of op stack
        ///     if op stack still has elements, check for * or / on top, Process * or / if necessary, pop, carry out op, push to value stack by calling valueCalc
        ///     catch (InvalidOperationException) This catches no operator no values errors and throws an argument exception
        ///     
        ///             /// The last token has been processed, the main loop has ended, now check empty stack conditions 
        /// 
        /// 6)  if the opStack is empty, Return the value stack pop as the final answer
        ///     catch (InvalidOperationException) Catches an invalid formula, throws an argument exception
        ///     
        /// 7)  if opStack has 1 operator, Conduct final operation by calling valueCalc, return answer as the final answer
        ///     catch (InvalidOperationException) Catches an invalid formula, throws an argument exception, not enough in value stack/op stack
        ///     
        /// 8)  if Operator stack is > 1, throw an argument exception for an invalid formula
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns>The answer to the expression</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="DivideByZeroException"></exception>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            if (expression == null) { throw new ArgumentException(); }
            expression = expression.Trim();
            bool didOp = false;
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>(); 
            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (string t in tokens)
            {
                string token = t.Trim();
                if (token == " ") { continue; }; 
                int num = 0;
                int.TryParse(token, out num); //1
                if ( num != 0 || token =="0") 
                {
                    checkMD(num);
                    continue;
                }
                if (token.Count() > 1) //2
                {
                    char firstChar = token[0];
                    char lastChar = token[token.Length - 1];
                    bool startLet = !(Char.IsDigit(firstChar));
                    bool endNum = (Char.IsDigit(lastChar));
                    if (startLet && endNum)
                    {
                        int foundValue = variableEvaluator(token.Trim());
                        checkMD(foundValue);
                    }
                }
                if (token == "+" || token == "-") //3
                {
                    if (operatorStack.Count > 0)
                    {
                        if (valueStack.Count > 1 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                        {
                            int answer = valueCalc(operatorStack.Peek(), false);
                            valueStack.Push(answer);
                        }
                        else
                        {
                            operatorStack.Push(token);
                        }
                    }
                    else
                    {
                        operatorStack.Push(token);
                        continue;
                    }
                }
                if (token == "*" || token == "/" || token == "(") //4
                {
                    operatorStack.Push(token);
                    didOp = false;
                    continue;
                }
                if (token == ")") //5
                {
                    
                    try
                    {
                        string operand = operatorStack.Peek();
                        if ((operand == "+" || operand == "-") && valueStack.Count > 1) 
                        {
                            int answer = valueCalc(operand, true);
                            valueStack.Push(answer);
                            didOp = true;
                        }

                        operand = operatorStack.Peek(); 
                        if (operand == "(" && didOp)
                        {
                            operatorStack.Pop();
                        }

                        if (operatorStack.Count > 0) 
                        {
                            operand = operatorStack.Peek();
                        }

                        if (operand == "*" || operand == "/" && valueStack.Count > 1) 
                        {
                            int answer = valueCalc(operand, true);
                            valueStack.Push(answer);
                        }
                    }
                    catch (InvalidOperationException) 
                    {
                        throw new ArgumentException();
                    }
                }
            }
            if (operatorStack.Count == 0) // 6
            {
                try
                {
                    return valueStack.Pop(); 
                }
                catch (InvalidOperationException) 
                {
                    throw new ArgumentException(); 
                }
            }
            if (operatorStack.Count == 1) // 7
            {
                try
                {
                    string operand = operatorStack.Pop();
                    if (operand == "+" ||  operand == "-")
                    {
                        try 
                        {
                            int answer = valueCalc(operand, false);
                            return answer;
                        }
                        catch (InvalidOperationException) 
                        {
                            throw new ArgumentException();
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException();
                }
            }
            throw new ArgumentException(); // 8

            ///<summary>takes both x and y values and the operand and returns the calculated integer, also prevents dividing by zero</summary>
            ///<param name="x">an int x</param>
            ///<param name="y"> an int y</param>
            ///<param name="operand">the operator used to calculate</param>
            ///<returns>the answer from evaluating (x operand y)</returns>
            int Calculate(int x, int y, string operand) 
            {
                if (operand == "/")
                {
                    if (y == 0)
                    {
                        throw new ArgumentException("Cannot Divide by 0");
                    }
                    return x / y;
                }
                if (operand == "*")
                {
                    return x * y;
                }
                if (operand == "+")
                {
                    return x + y;
                }
                if (operand == "-")
                {
                    if (y > 0)
                    {
                        return x - y;
                    }
                    else
                    {
                        return x + y;
                    }

                }
                throw new ArgumentException();
            }
            ///<summary>check for multiply or divide checks the operand stack for a '*' or '/' if it is, we pop the value stack once, and call calculate,
            ///then we push the answer to the value stack. If not, we just push the token onto the valueStack
            ///catch (InvalidOperationException) these catch if the value stack and/or operator stack has no values (indicating an incorrect formula)
            ///</summary>
            ///<param name="y">an int that represents a token</param>
            void checkMD(int y)
            {
                try 
                {
                    if (operatorStack.Peek() == "*" || operatorStack.Peek() == "/")
                    {
                        try 
                        {
                            int x = valueStack.Pop();
                            string operand = operatorStack.Pop();
                            int answer = Calculate(x, y, operand);
                            didOp = true;
                            valueStack.Push(answer); 
                        }
                        catch (InvalidOperationException)
                        {
                            throw new ArgumentException();
                        }
                    }
                    else
                    {
                        valueStack.Push(y); 
                    }
                }
                catch (InvalidOperationException) 
                {
                    valueStack.Push(y); 
                }
            }

            ///<summary>
            ///This function checks a condition, popOp, if a pop of the opStack is necessary, it then pops the value stack twice, and 
            ///calls the calculate function to do the arithmetic, then it returns the answer. 
            ///Note: this returns the final answer in some circumstances, and is just returned to be put on the value stack in others
            /// </summary>
            ///<param name="operand">a string of an operation ( + - * / )</param>
            ///<param name="popOp">an bool used to tell if a pop on the operator stack is necessary</param>
            ///<returns>the answer from evaluating (x operand y) by calling Calculate function</returns>
            int valueCalc(string operand, bool popOp)
            {
                if (popOp)
                {
                    operatorStack.Pop();
                }
                int y = valueStack.Pop();
                int x = valueStack.Pop();
                int answer = Calculate(x, y, operand);
                return answer;
            }
        }
    }
}

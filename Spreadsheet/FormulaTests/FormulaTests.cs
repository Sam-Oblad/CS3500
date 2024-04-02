
/// <summary>
/// Author: Sam Oblad
/// Partner: None
/// Date: 1/27/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// I, Sam Oblad, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
/// 
/// File Contents
/// This file contains my test cases for my formula class
/// </summary>

using Newtonsoft.Json.Linq;
using SpreadsheetUtilities;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {

        //! This Section is formula construction/evaluation tests
        /// <summary>
        /// Scientific notation test op from right
        /// </summary>
        [TestMethod]
        public void scientificNotationTest1()
        {
            Formula formula1 = new("5e2 + 1");
            object answer = formula1.Evaluate(s => 0);
            Assert.AreEqual(501.0, answer);

        }

        /// <summary>
        /// Scientific notation test op from left
        /// </summary>
        [TestMethod]
        public void scientificNotationTest2()
        {
            Formula formula1 = new("2 + 5e2");
            object answer = formula1.Evaluate(s => 0);
            Assert.AreEqual(502.0, answer);

        }

        /// <summary>
        /// Scientific notation test complex parens
        /// </summary>
        [TestMethod]
        public void scientificNotationTest3()
        {
            Formula formula1 = new("(4*5+6)*(5e2 + (5+8*9))");
            object answer = formula1.Evaluate(s => 0);
            Assert.AreEqual(15002.0, answer);

        }
        
        [TestMethod]
        public void divideByZeroTest1()
        {
            Formula formula1 = new("8/0");
            object response = formula1.Evaluate(s => 0);
            Assert.IsTrue(response is FormulaError);
        }

        /// <summary>
        /// Checks that divide by zero is throwing a formula format exception
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void divideByZeroTest2()
        {
            Formula formula1 = new Formula("1/0");
            object error = formula1.Evaluate(null);
            Assert.IsTrue(error is FormulaError);
        }

        /// <summary>
        /// Tests an unsuccessful variable lookup
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void noLookupVarTest()
        {
            Formula formula1 = new("Y1", normalizer, validator);
            object response = formula1.Evaluate(findVar);
            Assert.IsTrue(response is FormulaError);
        }

        /// <summary>
        /// Tests empty formula
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void emptyTest()
        {
            Formula formula1 = new("");
            formula1.Evaluate(s => 0);
        }

        /// <summary>
        /// tests formula evaluation with variables, parens, addition, subtraction, multiplication and division
        /// </summary>
        [TestMethod]
        public void massiveFormulaEvaluationTest1()
        {
            Formula formula1 = new("Var1 * (((98*7)-45)+(8*3-Var2))/8*9*(Var3+6)");
            Assert.AreEqual(41391.0, formula1.Evaluate(findVar));
        }

        /// <summary>
        /// tests formula evaluation with addition and subtraction in this one
        /// </summary>
        [TestMethod]
        public void massiveFormulaEvaluationTest2()
        {
            Formula formula1 = new("(((98+7)-45)+(8-3))+8+9-(6)");
            Assert.AreEqual(76.0, formula1.Evaluate(findVar));
        }

        //!This section tests various syntax errors
        /// <summary>
        /// Tests a bunch of garbage input for formula constructor
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest1()
        {
            Formula formula1 = new Formula("&%$#1", normalizer, validator);
        }

        /// <summary>
        /// tests a garbage value in string for formula constructor
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest2()
        {
            Formula formula1 = new Formula("^", normalizer, validator);
        }

        /// <summary>
        /// Tests that error is thrown for no var or double before operator in string for formula construction
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest3()
        {
            Formula formula1 = new Formula("+a2", normalizer, validator);
        }

        /// <summary>
        /// Tests that an error is thrown if uneven parens on right
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest4()
        {
            Formula formula1 = new Formula("(1+2))", normalizer, validator);
        }

        /// <summary>
        /// Tests that an error is thrown if uneven parens on left
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest5()
        {
            Formula formula1 = new Formula("((1+2)", normalizer, validator);
        }

        /// <summary>
        /// Tests that an error is thrown if an empty string is passed as the string argument in formula constructor
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest6()
        {
            Formula formula1 = new Formula("", normalizer, validator);
        }

        /// <summary>
        /// a FormulaFormatException error thrown by the validator because no op after parens is not valid
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void syntaxErrorNormalizerValidatorTest8()
        {
            Formula formula1 = new Formula("(1+2)5", normalizer, validator);
        }

        //!This section tests variable validation errors
        /// <summary>
        /// This throws a syntax error because the validator does not allow for variables that dont start with a letter and end with a number
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void variableValidatorTest1()
        {
            Formula formula1 = new Formula("_A2A/23", normalizer, validator);
        }

        /// <summary>
        /// This throws a syntax error because the validator does not allow for variables that dont start with a letter and end with a number
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void variableValidatorTest2()
        {
            Formula formula1 = new Formula("1_A2*123", normalizer, validator);
        }
        /// <summary>
        /// Tests using a validator to validate valid variables
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void variableValidatorTest3()
        {
            Formula formula1 = new Formula("2____A", normalizer, validator);
        }

        /// <summary>
        /// Tests using a validator to validate valid variables
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void variableValidatorTest4()
        {
            Formula formula1 = new Formula("1A", normalizer, validator);
        }

        //!This section tests constructions with normalizers and validators and without
        /// <summary>
        /// Catches a FormulaFormatException error thrown by the validator because "x" is not valid
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void createFormulaWithNormalizerAndValidatorFalseTest()
        {
            Formula formula1 = new Formula("x+y3", normalizer, validator);
        }

        /// <summary>
        /// Catches a FormulaFormatException error thrown because 2x is syntactically incorrect for a formula
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void createFormulaWithNormalizerAndValidatorSyntaxErrorTest()
        {
            Formula formula1 = new Formula("2x+y3", normalizer, validator);
        }

        /// <summary>
        /// Tests that evaluate returns formula error if variable cant be looked up 
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void invalidVariableEvaluateTest()
        {
            Formula formula1 = new Formula("x3 + 10");
            object error = formula1.Evaluate(findVar);
            Assert.IsTrue(error is FormulaError);
        }

        //!The following tests are based on real api test examples
        /// <summary>
        /// Simplest evaluation test, make sure basic computations are working correctly, not variables or parentheses involved
        /// </summary>
        [TestMethod]
        [TestCategory("No Validator/Normalizer")]
        public void simpleFormulaEvaluationTest()
        {
            Formula formula1 = new Formula("1 + 1");
            Formula formula2 = new Formula("1 - 1");
            Formula formula3 = new Formula("2 * 5");
            Formula formula4 = new Formula("50 / 10");
            Assert.AreEqual(formula1.ToString(), "1+1");
            Assert.AreEqual(formula2.ToString(), "1-1");
            Assert.AreEqual(formula3.ToString(), "2*5");
            Assert.AreEqual(formula4.ToString(), "50/10");
            Assert.AreEqual(formula1.Evaluate(findVar), 2.0);
            Assert.AreEqual(formula2.Evaluate(findVar), 0.0);
            Assert.AreEqual(formula3.Evaluate(findVar), 10.0);
            Assert.AreEqual(formula4.Evaluate(findVar), 5.0);
        }

        /// <summary>
        /// Creates an instance of the formula class without passing a normalizer or validator
        /// </summary>
        [TestMethod]
        public void createFormulaNoNormalizerOrValidatorTest()
        {
            Formula formula1 = new Formula("1+y1");
            string response = formula1.ToString();
            Assert.AreEqual(response, "1+y1");
        }

        //The following tests require a normalizer that changes the string to uppercase, and a validator that
        //that returns true only if a string consists of one letter followed by one digit
        /// <summary>
        /// Creates an instance of the formula class and passes a normalizer(toUpper) and validator(OneLetterOneDigit)
        /// </summary>
        [TestMethod]
        public void createFormulaWithNormalizerAndValidatorTest()
        {
            Formula formula1 = new Formula("x2+y3", normalizer, validator);
            string response = formula1.ToString();
            Assert.AreEqual(response, "X2+Y3");
        }

        /// <summary>
        /// Tests evaluateFormulaTest
        /// </summary>
        [TestMethod]
        public void evaluateFormulaTest()
        {
            Formula formula1 = new Formula("x+7", normalizer, s => true);
            Formula formula2 = new Formula("x+7");
            object answer1 = formula1.Evaluate(findVar);
            object answer2 = formula2.Evaluate(findVar);
            Assert.AreEqual(answer1, 11.0);
            Assert.AreEqual(answer2, 9.0);
        }

        /// <summary>
        /// Tests the enumerateFormula method, first checks that basics of it are working, i.e. normalizer functions correctly,
        /// then checks to make sure that enumerated strings are not appearing more than once,
        /// then checks use without a normalizer, allowing for a lowercase letter and uppercase letter since they are not identical
        /// </summary>
        [TestMethod]
        public void enumerateFormulaTest()
        {
            HashSet<string> correct = new() { "X", "Y", "Z", "VAR2" };
            Formula formula1 = new Formula("x+y*z+var2", normalizer, s => true);
            IEnumerable<String> answer = formula1.GetVariables();
            Assert.IsTrue(correct.SetEquals(answer));

            correct = ["X", "Z"];
            Formula formula2 = new Formula("x+X*z", normalizer, s => true);
            answer = formula2.GetVariables();
            Assert.IsTrue(correct.SetEquals(answer));

            correct = ["x", "X", "z", "var2"];
            Formula formula3 = new Formula("x+X*z+var2");
            answer = formula3.GetVariables();
            Assert.IsTrue(correct.SetEquals(answer));
        }

        /// <summary>
        /// Tests that toString override is working correctly, checks both with and without a normalizer
        /// </summary>
        [TestMethod]
        public void toStringTest()
        {
            Formula formula1 = new Formula("x + y *         8", normalizer, s => true);
            string answer = formula1.ToString();
            Assert.AreEqual("X+Y*8", answer);

            Formula formula2 = new Formula("x + Y / 10");
            answer = formula2.ToString();
            Assert.AreEqual("x+Y/10", answer);


        }

        /// <summary>
        /// Tests equality override between formulas. Uses both normalizer and no normalizer
        /// </summary>
        [TestMethod]
        public void formulaEqualityTest()
        {
            Formula formula1 = new Formula("x1+y2", normalizer, s => true);
            Assert.IsTrue(formula1.Equals(new Formula("X1  +  Y2")));

            Formula formula2 = new Formula("x1+y2");
            Assert.IsFalse(formula2.Equals(new Formula("X1+Y2")));

            Formula formula3 = new Formula("x1+y2");
            Assert.IsFalse(formula3.Equals(new Formula("y2+x1")));

            Formula formula4 = new Formula("2.0000 + x7");
            Assert.IsTrue(formula4.Equals(new Formula("2.0000000000 + x7")));
        }

        /// <summary>
        /// Tests that false returns for .equals when null or a nonformula type are provided
        /// </summary>
        [TestMethod]
        public void nonFormulaAndNullFormulaEqualityTest()
        {
            Formula formula1 = new Formula("x1+y2", normalizer, s => true);
            Assert.IsFalse(formula1.Equals(null));
            Assert.IsFalse(formula1.Equals(1));
        }

        /// <summary>
        /// Tests getvariables method
        /// </summary>
        [TestMethod]
        public void getVariablesTest()
        {
            Formula formula1 = new Formula("Var1+VAR1*z", normalizer, s => true); //should enumerate "VAR1", "VAR1", and "Z"
            Formula formula2 = new Formula("Var2+X*z", normalizer, s => true); //should enumerate "VAR2" and "Z".
            Formula formula3 = new Formula("Var3+X*z+VAR3"); //should enumerate "Var3", "X", "VAR3" and "z".
            HashSet<string> answers = new();
            answers.Add("VAR1");
            answers.Add("Z");
            Assert.IsTrue(answers.SetEquals(formula1.GetVariables()));
            answers.Clear();

            answers.Add("VAR2");
            answers.Add("X");
            answers.Add("Z");
            Assert.IsTrue(answers.SetEquals(formula2.GetVariables()));
            answers.Clear();

            answers.Add("Var3");
            answers.Add("VAR3");
            answers.Add("X");
            answers.Add("z");
            Assert.IsTrue(answers.SetEquals(formula3.GetVariables()));
        }

        //!Equals Tests
        /// <summary>
        /// Tests that false returns for .equals when uneven formulas are provided
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityTest1()
        {
            Formula formula1 = new Formula("x1+x2", normalizer, s => true);
            Formula formula2 = new("x1+x2+x3", normalizer, s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that false returns for .equals when uneven formulas are in different orders
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityTest2()
        {
            Formula formula1 = new Formula("2.0+x2", normalizer, s => true);
            Formula formula2 = new("x2+2.0", normalizer, s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that false returns for .equals when  formulas have different doubles but same vars
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityTest3()
        {
            Formula formula1 = new Formula("2.0+x1", normalizer, s => true);
            Formula formula2 = new("3.0+x1", normalizer, s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that false returns for .equals when  variables are different cases 
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityTest4()
        {
            Formula formula1 = new Formula("2.0+x1", normalizer, s => true);
            Formula formula2 = new("2.0+x1", s=>s, s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that false returns for .equals when  formulas have different doubles but same vars
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityTest5()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(y48- _32 *24.001+(5-4)-Var2)", normalizer, s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that true returns for .equals when  formulas have extended doubles but same vars
        /// </summary>
        [TestMethod]
        public void equalityTest6()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(X48- _32 *24.0000000+(5-4)-Var2)", normalizer, s => true);
            Assert.IsTrue(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests that true returns for .equals when formulas have extended doubles same vars
        /// </summary>
        [TestMethod]
        public void equalityTest7()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.001", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.001", normalizer, s => true);
            Assert.IsTrue(formula1.Equals(formula2));
        }

        //!Test for ==
        /// <summary>
        /// Tests that true returns for equal formulas using == also tests that hashcodes are equal
        /// </summary>
        [TestMethod]
        public void equalityOpTest1()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(X48- _32 *24.0000000+(5-4)-Var2)", normalizer, s => true);
            Assert.IsTrue(formula1 == formula2);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests that true returns for equal formulas using == also tests that hashcodes are equal
        /// </summary>
        [TestMethod]
        public void equalityOpTest2()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.001", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.001", normalizer, s => true);
            Assert.IsTrue(formula1 == formula2);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests that false returns for unequal formulas using == also tests that hashcodes are not equal
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void equalityOpTest3()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(y48- _32 *24.001+(5-4)-Var2)", normalizer, s => true);
            Assert.IsFalse(formula1 == formula2);
            Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        //!Test for !=
        /// <summary>
        /// Tests that false returns for != when formulas are identical also tests that hashcodes are equal
        /// </summary>
        [TestMethod]
        public void inequalityOpTest1()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(X48- _32 *24.0000000+(5-4)-Var2)", normalizer, s => true);
            Assert.IsFalse(formula1 != formula2);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests that false returns for != when formulas are identical also tests that hashcodes are equal
        /// </summary>
        [TestMethod]
        public void inequalityOpTest2()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.001", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.001", normalizer, s => true);
            Assert.IsFalse(formula1 != formula2);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests a return of true since the formulas do not match also tests that hashcodes are unequal
        /// </summary>
        [TestMethod]
        [TestCategory("Error Handling")]
        public void inequalityOpTest3()
        {
            Formula formula1 = new Formula("Var1+(x48- _32 *24+(5-4)-Var2)", normalizer, s => true);
            Formula formula2 = new("Var1+(y48- _32 *24.001+(5-4)-Var2)", normalizer, s => true);
            Assert.IsTrue(formula1 != formula2);
            Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        //!Tests for getHashCode()
        /// <summary>
        /// Tests that hash codes are identical for same formula, different objects
        /// </summary>
        [TestMethod]
        public void hashTest1()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.001", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.001", normalizer, s => true);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests that hash codes are identical for same formula, different objects, and an extended double
        /// </summary>
        [TestMethod]
        public void hashTest2()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.0000000", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.0", normalizer, s => true);
            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        /// <summary>
        /// Tests that hash codes are not identical for different formula, extended .001 on double
        /// </summary>
        [TestMethod]
        public void hashTest3()
        {
            Formula formula1 = new Formula("var1+var2-var3*2.001", normalizer, s => true);
            Formula formula2 = new("var1+var2-var3*2.0", normalizer, s => true);
            Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        //!This section is for my helper methods
        /// <summary>
        /// Normalizer used throughout the api examples implemented appropriately
        /// </summary>
        /// <param name="input">a token</param>
        /// <returns>the normalized token</returns>
        private string normalizer(string input)
        {
            return input.ToUpper();
        }

        /// <summary>
        /// Checks for appropriate variable insertions, also checks for operators and doubles
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool validator(string input)
        {
            if (input == "+" || input == "-" || input == "*" || input == "/" || input == "(" || input == ")") { return true; }
            try
            {
                double response = double.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                char firstChar = input[0];
                char lastChar = input[input.Length - 1];
                bool startLet = !(Char.IsDigit(firstChar));
                bool endNum = (Char.IsDigit(lastChar));
                if (startLet && endNum)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// A rudimentary lookup function to be passed as a delegate
        /// </summary>
        /// <param name="input">token</param>
        /// <returns>double representing token's value</returns>
        /// <exception cref="ArgumentException"></exception>
        private double findVar(string input)
        {
            Dictionary<string, double> values = new();
            values.Add("x", 2);
            values.Add("X", 4);
            values.Add("Var1", 4);
            values.Add("Var2", 8);
            values.Add("Var3", 8);

            foreach (KeyValuePair<string, double> kvp in values)
            {
                if (input == kvp.Key)
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Unable to identify value of variable {input}");
        }
    }
}
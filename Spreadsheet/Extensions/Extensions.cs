using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace Extensions
{
    /// <summary>
    /// Extensions for Spreadsheet solution
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks if the target item is on top of the stack.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the stack.</typeparam>
        /// <param name="stack">The stack to check.</param>
        /// <param name="target">The target item to compare with the top item of the stack.</param>
        /// <returns>True if the target is on top of the stack, false otherwise.</returns>
        public static bool OnTop<T>(this Stack<T> stack, T target) where T : notnull
        {
            try
            {
                return stack.Peek().Equals(target);
            }
            catch { return false; }
        }

        /// <summary>
        /// Determines if a string is a positive number, either integer, decimal, or exponential 
        /// </summary>
        /// <param name="token"> string to be evaluated </param>
        /// <returns> True if token is an number, False if anything else </returns>
        public static bool IsNumber(string token) 
        {
            if (double.TryParse(token, out double result))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if a string is a variable 
        /// </summary>
        /// <param name="token"> string to be evaluated </param>
        /// <returns> True if token is a variable, False if anything else </returns>
        public static bool IsVariable(string token)
        {
            string pattern = "^[a-zA-Z_]([0-9a-zA-Z_]+)?$";
            if (Regex.IsMatch(token, pattern)) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Determines if a string is an operator: +, -, *, /, (, or )
        /// </summary>
        /// <param name="token"> string to be evaluated </param>
        /// <returns> True if token is an operator, False if anything else </returns>
        public static bool IsOperator(string token)
        {
            if (token == "*" || token == "/" || token == "+" || token == "-" || token == "(" || token == ")") { return true; }
            else return false;
        }
    }
}

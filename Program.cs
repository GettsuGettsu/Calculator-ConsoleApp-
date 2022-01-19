using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gettsu.Calculator
{
    class Calculator
    {
        /// <summary>
        /// Current position of a cursor.
        /// </summary>
        private static int currentCursorLeft = 0;

        /// <summary>
        /// Checks if a character is an operator.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        static bool IsOperator(char character)
        {
            if (character == '+' || character == '-' || character == '*' || character == '/' || character == '^')
                return true;
            return false;
        }

        /// <summary>
        /// Checks if the last character in a string is an operator.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        static bool IsOperator(string expression)
        {
            char character = expression[expression.Length - 1];
            if (character == '+' || character == '-' || character == '*' || character == '/' || character == '^')
                return true;
            return false;
        }

        /// <summary>
        /// Checks if a character is a bracket.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        static bool IsBracket(char character)
        {
            if (character == '(' || character == ')')
                return true;
            return false;
        }

        /// <summary>
        /// Clear the line under an active line.
        /// </summary>
        static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.CursorLeft = 0;
            Console.CursorTop--;
        }

        /// <summary>
        /// Delete character from a console and return the changed line as a string.
        /// </summary>
        /// <param name="lineContent"></param>
        /// <returns></returns>
        static string DeleteChar(string lineContent)
        {
            if (lineContent.Length > 0)
            {
                lineContent = lineContent.Substring(0, lineContent.Length - 1);
                ClearLine();
                Console.Write(lineContent);
                currentCursorLeft -= 1;
            }
            return lineContent;
        }

        /// <summary>
        /// Method is obsolete.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string InputValidate(string str)
        {
            //str = DeleteChar(str);
            return str;
        }

        static void Main(string[] args)
        {
            string expression = "";           
            
            while (true)
            {
                var currentChar = Console.ReadKey();

                if (currentChar.Key == ConsoleKey.Backspace)
                {
                    expression = DeleteChar(expression);   
                }

                #region Left&Right arrow keys Placeholder
                /*else if (currentChar.Key == ConsoleKey.LeftArrow)
                {
                    Left();
                } 
                else if (currentChar.Key == ConsoleKey.RightArrow)
                {
                    Right();
                }*/
                #endregion

                else if (currentChar.Key == ConsoleKey.Enter)
                {
                    expression = "";
                    ClearLine();
                    continue;
                }

                if (currentChar.KeyChar == ' ')
                {
                    expression += ' ';
                    expression = DeleteChar(expression);
                    continue;
                }
                else expression += currentChar.KeyChar;

                currentCursorLeft = expression.Length;

                if (IsOperator(currentChar.KeyChar))
                {
                    if (expression.Length > 1 && IsOperator(expression[expression.Length - 2]))
                    {
                        expression = DeleteChar(expression);
                    }

                    for (int i = 0; i < expression.Length - 1; i++)
                    {
                        //leftOperand += expressionString[i]; 
                    }
                }
                else if (!double.TryParse(currentChar.KeyChar.ToString(), out _) && currentChar.KeyChar != '(' && currentChar.KeyChar != ')')
                {
                    expression = DeleteChar(expression);
                }                

                // Show result at next line and go back to input line
                Console.CursorTop++;
                ClearLine();
                Console.Write(Parse(expression));
                Console.CursorTop--;
                Console.CursorLeft = currentCursorLeft;
            }
            
        }
        //
        // идея - попробовать реализивать через два массива: массив чисел и массив операторов (включая скобки)
        //
        static double Parse(string expressionString, double result = 0)
        {
            List<char> operators = new List<char>();
            List<double?> operands = new List<double?>();
            operands.Add(null);

            string tempString = "";
            int counter = 0;

            for (int i = 0; i < expressionString.Length; i++)
            {
                #region Старый вариант
                if (IsBracket(expressionString[i]) || IsOperator(expressionString[i]))
                {
                    operands.Add(null);
                    operators.Add(expressionString[i]);
                    tempString = "";
                    counter++;
                }
                else
                {
                    tempString += expressionString[i];
                    operands[counter] = double.Parse(tempString);
                }
                #endregion
            }            
            
            operands = operands.Where(x => x != null).ToList();

            result = Calculate(expressionString, operators, operands);
            // For testing purposes
            WriteToFile(expressionString, operators, operands);

            return result;
        }

        private static double Calculate(string expression, List<char> operators, List<double?> operands)
        {
            double result = 0;

            int leftBracketIndex;
            int rightBracketIndex;

            while (!double.TryParse(expression, out _))
            {
                //leftBracketIndex = operators.LastIndexOf('(');                
                leftBracketIndex = expression.LastIndexOf('(');                
                if (leftBracketIndex == -1)
                    //CalculateWOBrackets(expression, operators, operands);
                    expression = CalculateWOBrackets(expression, operators, operands).ToString();
                else
                {                    
                    //rightBracketIndex = operators.IndexOf(')', leftBracketIndex);
                    rightBracketIndex = expression.IndexOf(')', leftBracketIndex);
                    if (rightBracketIndex == -1)
                        return 0;

                    string expressionInBrackets = expression.Substring(++leftBracketIndex, rightBracketIndex - leftBracketIndex);
                    result = CalculateWOBrackets(expressionInBrackets, operators, operands);
                    expression = expression.Replace($"({expressionInBrackets})", result.ToString());
                    //expression = CalculateWOBrackets(expressionInBrackets, operators, operands).ToString();
                }
            }

            return double.Parse(expression);
        }
        //
        // Placeholder
        //
        private static double CalculateWOBrackets(string expression, List<char> operators, List<double?> operands)
        {
            double result = 0;
            
            // Need to rewrite
            while (!double.TryParse(expression, out result))
            {
                int operatorIndex = expression.IndexOf('^');
                try
                {
                    if (operatorIndex != -1)
                        expression = expression.Replace($"{operands[operatorIndex - 1]}^{operands[operatorIndex]}", Math.Pow((double)operands[operatorIndex - 1], (double)operands[operatorIndex]).ToString());
                    else break;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Write an expression to the text file.
        /// </summary>
        /// <param name="expression">The expression which will be written to a file.</param>
        /// <param name="operators">The operators in an expression.</param>
        /// <param name="operands">The operands in an expression.</param>
        private static void WriteToFile(string expression, List<char> operators, List<double?> operands)
        {
            if (File.Exists("arrays.txt"))
                File.Delete("arrays.txt");
            using (StreamWriter sw = new StreamWriter("arrays.txt"))
            {
                for (int i = 0; i < expression.Length; i++)
                    sw.Write(expression[i]);
                sw.WriteLine();
                for (int i = 0; i < operands.Count; i++)
                    sw.Write(operands[i] + " ");
                sw.WriteLine();
                for (int i = 0; i < operators.Count; i++)
                    sw.Write(operators[i]);
                sw.Dispose();
            }
        }
    }
}
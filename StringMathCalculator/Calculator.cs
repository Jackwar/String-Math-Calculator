using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using StringMathCalculator.Calculations;

namespace StringMathCalculator
{

    public class Calculator
    {
        ///<value>Regex for removing spaces from a string.</value>
        private static readonly Regex removeSpaces = new Regex(@"\t |\n |\r |\s", RegexOptions.Compiled);
        /// <value>List of reserved characters that cannot be assigned to an operation.</value>
        private static readonly List<char> reservedChars = new List<char>()
        {
            '(', ')', '.'
        };
        ///<value>StringBuilder holding the legal characters that can be in a calculation string.</value>
        private readonly StringBuilder legalCharacters;

        private readonly Dictionary<char, CalculatorOperation> singleCharOperations;
        private readonly Dictionary<char, string> wordReplacements;

        public Calculator()
        {
            legalCharacters = new StringBuilder();
            legalCharacters.Append(@"[^0-9()");

            singleCharOperations = new Dictionary<char, CalculatorOperation>()
            {
                { '^', new CalculatorOperation(Exponent, 2 ) },
                { '*', new CalculatorOperation(Times, 1) },
                { '+', new CalculatorOperation(Add, 0) },
                { '-', new CalculatorOperation(Minus, 0) },
                { '/', new CalculatorOperation(Divide, 1) },
                { 'r', new CalculatorOperation(Root, 2) },
                { '_', new CalculatorOperation(Log, 2) }
            };

            foreach(var entry in singleCharOperations)
            {
                if(entry.Value.RegexCharacter != null)
                {
                    legalCharacters.Append(entry.Value.RegexCharacter);
                }
                else
                {
                    legalCharacters.Append(entry.Key);
                }
            }

            wordReplacements = new Dictionary<char, string>()
            {
                { '_', "log" }
            };
        }

        /// <summary>
        /// Add a custom operation for the sum of two numbers.
        /// </summary>
        /// <param name="calculation">The method that takes two doubles and returns a double.</param>
        /// <param name="weight">The weight of the operation.</param>
        /// <param name="character">The character tied to the operation.</param>
        /// <exception cref="ArgumentException">
        /// Throws when a reserved charcter of '(', ')' or '.' is used, if the character is already in use or the character is a number.
        /// </exception>
        public void AddOperation(CalculationPair calculation, int weight, char character)
        {
            if (int.TryParse(character.ToString(), out _))
            {
                throw new ArgumentException("Character cannot be a number.");
            }
            if (reservedChars.Contains(character))
            {
                throw new ArgumentException("Reserved character, please avoid '(', ')' and '.'");
            }
            if (!singleCharOperations.ContainsKey(character))
            {
                singleCharOperations.Add(character, new CalculatorOperation(calculation, weight));
                if (character != '\\')
                {
                    legalCharacters.Append(character);
                }
                else
                {
                    legalCharacters.Append(@"\\");
                }
            }
            else if(WordReplacementsContains(character))
            {
                singleCharOperations.Add(character, new CalculatorOperation(calculation, weight));
            }
            else
            {
                throw new ArgumentException("Character is already in use for another operation.");
            }
        }

        /// <summary>
        /// Add a custom operation for the sum of two numbers. 
        /// <para>Takes a regex escape string if the character must be escaped to be in a regex.</para>
        /// </summary>
        /// <param name="calculation">The method that takes two doubles and returns a double.</param>
        /// <param name="weight">The weight of the operation.</param>
        /// <param name="character">The word tied to the operation. Can't contain spaces.</param>
        /// <exception cref="ArgumentException">
        /// Throws if the word is already in use, has spaces or is just a number.
        /// </exception>
        public void AddOperation(CalculationPair calculation, int weight, string word)
        {
            if(int.TryParse(word, out _))
            {
                throw new ArgumentException("Word can contain numbers, but must not be just a number.");
            }
            if(removeSpaces.IsMatch(word))
            {
                throw new ArgumentException("Word cannot have spaces.");
            }
            if (!wordReplacements.ContainsValue(word))
            {
                var character = AddUnusedCharacter(word);
                singleCharOperations.Add(character, new CalculatorOperation(calculation, weight));
                legalCharacters.Append(character);
            }
            else
            {
                throw new ArgumentException("Word is already in use for another operation.");
            }
        }
        /// <summary>
        /// Checks if a character exists in the wordReplacements Dictionary.
        /// </summary>
        /// <param name="character">Character to be checked for.</param>
        /// <returns>Bool for if character exists in wordReplacements</returns>
        private bool WordReplacementsContains(char character)
        {
            if (wordReplacements.ContainsKey(character))
            {
                string word = wordReplacements[character];
                CalculatorOperation operation = singleCharOperations[character];

                wordReplacements.Remove(character);
                singleCharOperations.Remove(character);
                var newChar = AddUnusedCharacter(word, character + 1);

                singleCharOperations.Add(newChar, operation);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Find an unused character, starting from unicode 161.
        /// </summary>
        /// <param name="word">Word to be added to wordReplacements Dictionary.</param>
        /// <param name="startPoint">Unicode starting point, default is 161.</param>
        /// <returns>The found unused character.</returns>
        private char AddUnusedCharacter(string word, int startPoint = 161)
        {
            char replacement = (char)startPoint;

            if(singleCharOperations.ContainsKey(replacement))
            {
                for (int i = 0; i < 1023; i++)
                {
                    replacement++;
                    if(!singleCharOperations.ContainsKey(replacement))
                    {
                        break;
                    }
                }
            }

            legalCharacters.Append(replacement);
            wordReplacements.Add(replacement, word);
            return replacement;

        }
          
        //Default methods for calculations. 
             
        private double Exponent(double x, double y)
        {
            return Math.Pow(x, y);
        }

        private double Times(double x, double y)
        {
            return x * y;
        }

        private double Add(double x, double y)
        {
            return x + y;
        }

        private double Minus(double x, double y)
        {
            return x - y;
        }

        private double Divide(double x, double y)
        {
            return x / y;
        }

        private double Root(double x, double y)
        {
            return Math.Pow(x, 1/y);
        }

        private double Log(double x, double y)
        {
            return Math.Log(x, y);
        }

        ///<summary>
        ///<para>The main method for calculations.</para>
        ///<para>Reads <paramref name="calculations"/> from right to left and parses the characters to doubles and operations.</para>
        ///</summary>
        ///<returns>
        ///The calculations ready to be calculated in order.
        ///</returns>
        ///<exception cref="System.FormatException">
        ///Thrown when a string provided has illegal characters, too many parentheses or too many decimals in a number.
        ///</exception>
        ///<param name="calculations">A string with math calculations.</param>
        ///
        public CalculatorCalculation Calculation(string calculations)
        {
            //StringBuilder builder = new StringBuilder(calculations.ToLower());
            //builder.Replace(removeSpaces, string.Empty);
            calculations = removeSpaces.Replace(calculations, string.Empty);

            foreach(var wordReplacement in wordReplacements)
            {
                calculations = calculations.Replace(wordReplacement.Value, wordReplacement.Key.ToString());
            }

            if(Regex.IsMatch(calculations, legalCharacters.ToString() + "]"))
            {
                throw new FormatException("Illegal characters in calculation.");
            }

            var calculationHelper = new CalculationHelper();

            for (int i = calculations.Length - 1; i > -1; i--)
            {
                bool isNum = int.TryParse(calculations[i].ToString(), out _);

                //If the character is a number add it to the numberChar queue
                if (isNum)
                {
                    calculationHelper.OperationNext = true;
                    calculationHelper.NumberChar.Push(calculations[i]);
                    //Check if we are on the end of the calculations string
                    //If we are, add a SingleNum to the operation order with the number
                    if (i == 0)
                    {
                        string numberString = new string(calculationHelper.NumberChar.ToArray());
                        calculationHelper.OperationOrder.Enqueue(new OperationNum(double.Parse(numberString)));
                    }
                }
                else if (calculations[i].Equals(')'))
                {
                    RightParentheses(calculationHelper);
                }
                else if (calculations[i].Equals('('))
                {
                    i = LeftParentheses(calculations, calculationHelper, i);
                }
                else if (calculations[i].Equals('.'))
                {
                    Decimal(calculations, calculationHelper, i);
                }
                else if(!calculationHelper.OperationNext)
                {
                    throw new FormatException("Operation found next to another operation.");
                }
                else
                {
                    //Check if the number is negative
                    if(calculations[i].Equals('-'))
                    {
                        if(i == 0)
                        {
                            calculationHelper.NumberChar.Push(calculations[i]);

                            calculationHelper.OperationOrder.Enqueue(
                                new OperationNum(
                                    double.Parse(new string(calculationHelper.NumberChar.ToArray()))));

                            continue;
                        }
                        else
                        {
                            char leftCharacter = calculations[i - 1];

                            bool isLeftNum = int.TryParse(leftCharacter.ToString(), out _);

                            if(!isLeftNum  
                                && !leftCharacter.Equals(')'))
                            {
                                calculationHelper.NegativeNum = true;
                                calculationHelper.NumberChar.Push(calculations[i]);

                                continue;
                            }
                        }
                    }

                    if(i == 0)
                    {
                        throw new FormatException("Operation found at the beginning of the string");
                    }

                    double number = double.Parse(new string(calculationHelper.NumberChar.ToArray()));

                    calculationHelper.Reset();

                    //var (calculationPair, weight) = GetCalculationPairAndWeight(calculations[i]);

                    var calculatorOperation = singleCharOperations[calculations[i]];

                    calculationHelper.OperationOrder.Enqueue(
                        new OperationPair(number,
                                      calculatorOperation.Weight,
                                      calculatorOperation.CalculationPair));
                }
            }

            return OrderCalculations(calculationHelper.OperationOrder);
        }

        private static void Decimal(string calculations, CalculationHelper calculationHelper, int i)
        {
            if (!calculationHelper.FirstDecimal)
            {
                throw new FormatException("Found more than one decimal point in a single number.");
            }

            if (calculationHelper.NumberChar.Count != 0)
            {
                if (calculationHelper.NegativeNum && calculationHelper.NumberChar.Count == 1)
                {
                    calculationHelper.NumberChar.Push('0');
                }
                calculationHelper.NumberChar.Push(calculations[i]);
                calculationHelper.FirstDecimal = false;
            }
            else
            {
                calculationHelper.NumberChar.Push('0');
                calculationHelper.NumberChar.Push(calculations[i]);
                calculationHelper.FirstDecimal = false;
            }
        }

        private int LeftParentheses(string calculations, CalculationHelper calculationHelper, int i)
        {
            //Check if we have right parentheses in the string for the left parentheses to close.
            //Also check if there is a number waiting to be passed.
            if (calculationHelper.ParenthesesNum > 0 && calculationHelper.NumberChar.Count != 0)
            {

                //Queue the number that is in numberChar
                calculationHelper.OperationOrder.Enqueue(
                    new OperationNum(
                        double.Parse(new string(calculationHelper.NumberChar.ToArray()))));

                calculationHelper.Reset();

                //Check if we are at the end of the string or not
                if (i > 0)
                {
                    int i2;
                    //Loop for numbers or operations after the left parentheses.
                    for (i2 = i; i2 > 0 && calculationHelper.ParenthesesNum > 0; i2--)
                    {
                        calculationHelper.ParenthesesNum--;
                        char leftCharacter = calculations[i2 - 1];

                        if (leftCharacter.Equals('('))
                        {
                            var operationParentheses = new OperationParentheses(Parentheses.LEFT);
                            calculationHelper.OperationOrder.Enqueue(operationParentheses);
                        }
                        //Check if there is a number, right parentheses or operation left of the left parentheses
                        else
                        {
                            bool isLeftNum = int.TryParse(leftCharacter.ToString(), out _);

                            if (isLeftNum || leftCharacter.Equals(')'))
                            {
                                var calculationOperation = singleCharOperations['*'];
                                OperationParentheses operationParentheses = new OperationParentheses(Parentheses.LEFT)
                                {
                                    calcPair = calculationOperation.CalculationPair,
                                    Weight = calculationOperation.Weight
                                };
                                calculationHelper.OperationOrder.Enqueue(operationParentheses);

                                break;
                            }
                            else if (leftCharacter.Equals('('))
                            {
                                var operationParentheses = new OperationParentheses(Parentheses.LEFT);
                                calculationHelper.OperationOrder.Enqueue(operationParentheses);
                            }
                            else
                            {
                                var calculationOperation = singleCharOperations[leftCharacter];
                                OperationParentheses operationParentheses = new OperationParentheses(Parentheses.LEFT)
                                {
                                    calcPair = calculationOperation.CalculationPair,
                                    Weight = calculationOperation.Weight
                                };
                                calculationHelper.OperationOrder.Enqueue(operationParentheses);

                                if (leftCharacter.Equals('g'))
                                {
                                    i2 -= 2;
                                }

                                i = i2 - 1;

                                break;
                            }
                        }
                    }

                    //Check if the end of the string had closed all parentheses.
                    if (i2 == 0 && calculationHelper.ParenthesesNum == 1)
                    {
                        var operationParentheses = new OperationParentheses(Parentheses.LEFT);
                        calculationHelper.OperationOrder.Enqueue(operationParentheses);

                        i = 0;
                    }
                    else if (i2 == 0 && calculationHelper.ParenthesesNum > 0)
                    {
                        throw new FormatException("Parentheses not all closed at start of string.");
                    }
                }
                else
                {
                    var operationParentheses = new OperationParentheses(Parentheses.LEFT);
                    calculationHelper.OperationOrder.Enqueue(operationParentheses);
                }
            }
            else
            {
                if (calculationHelper.NumberChar.Count == 0)
                {
                    throw new FormatException("Operation found at the end of parentheses");
                }
                else
                {
                    throw new FormatException("Left parentheses found without enclosing right parentheses");
                }
            }

            return i;
        }

        private void RightParentheses(CalculationHelper calculationHelper)
        {
            calculationHelper.ParenthesesNum++;

            //Check if there is a number waiting to be passed in numberChar
            if (calculationHelper.NumberChar.Count != 0)
            {
                if (calculationHelper.NegativeNum && calculationHelper.NumberChar.Count == 1)
                {
                    throw new FormatException("Double negative found in front of parentheses.");
                }

                double number = double.Parse(new string(calculationHelper.NumberChar.ToArray()));

                calculationHelper.Reset();

                //var (calculationPair, weight) = GetCalculationPairAndWeight('*');
                var calculationOperation = singleCharOperations['*'];
                calculationHelper.OperationOrder.Enqueue(
                    new OperationPair(number,
                                  calculationOperation.Weight,
                                  calculationOperation.CalculationPair));
            }

            calculationHelper.OperationOrder.Enqueue(new OperationParentheses(Parentheses.RIGHT));
        }

        /// <summary>
        /// Reorder the operations in the opertionsOrder Queue by weight of operation
        /// </summary>
        /// <param name="operationOrder">Queue of operations</param>
        /// <returns>CalculatorCalculation ready for calculations to be executed</returns>
        private CalculatorCalculation OrderCalculations(Queue<object> operationOrder)
        {
            //The originCalculator is the top of the binary tree structure for the operations order
            var originCalculator = new CalculatorCalculation();
            //The current calculation, start with the origin
            var currentCalculator = originCalculator;

            //The stack for determining the current parentheses we are in
            var parenthesesStack = new Stack<CalculatorCalculation>();

            //Loop through all the opearations and order them according to the order of operations
            foreach(object operation in operationOrder)
            {
                //Put the number associated with the operation to the right of the tree as a single number
                if (operation is OperationCalc operationCalc)
                {
                    var rightCalculator = new CalculatorNumber(operationCalc.X);
                    currentCalculator.Right = rightCalculator;

                    if (operation is OperationPair operationPair)
                    {
                        currentCalculator.PairCalc = operationPair.CalcPair;
                        currentCalculator.Weight = operationPair.Weight;
                    }
                    else if (operation is OperationNum)
                    {
                        //If the Operation is an OperationNum, don't assign a calculation
                        //and make the weight -1 so its operation order is never checked
                        currentCalculator.Weight = -1;
                    }
                }
                //Operation is an OperationParentheses
                else
                {
                    var operationParentheses = (OperationParentheses)operation;

                    //Check if operation is a right parentheses
                    if(operationParentheses.Parentheses == Parentheses.RIGHT)
                    {
                        //Set the parentheses calculation with a weight -1 so its operation order is never checked
                        currentCalculator.Weight = -1;
                        currentCalculator.Right = new CalculatorCalculation()
                        {
                            Top = currentCalculator
                        };
                        parenthesesStack.Push(currentCalculator);

                        currentCalculator = (CalculatorCalculation) currentCalculator.Right;

                        continue;
                    }
                    //The operation is a left parentheses
                    else
                    {
                        var rightParentheses = parenthesesStack.Pop();
                        
                        if(operationParentheses.calcPair != null)
                        {
                            rightParentheses.Weight = operationParentheses.Weight;
                            rightParentheses.PairCalc = operationParentheses.calcPair;
                            currentCalculator = rightParentheses;
                        }
                        else
                        {
                            var tempTop = (CalculatorCalculation)rightParentheses.Top;
                            //If extraneous parentheses surround the given string operation the top calculation
                            //will end up as a parentheses, and subsequently have no left operation
                            //Check if there is a calculation above the current parentheses, otherwise don't add to left.
                            if (tempTop != null)
                            {
                                tempTop.Left = rightParentheses;
                            }

                            continue;
                        }
                    }
                }

                //The CalculatorCalculation above the current CalculationCalculator in the binary tree
                var topCalculator = (CalculatorCalculation)currentCalculator.Top;

                //Check if we are on the originCalculator
                if(topCalculator != null)
                {
                    //Don't change operations order if the currentCalculator is a OperationNum
                    if(topCalculator.Weight != -1 && currentCalculator.Weight != -1)
                    {
                        //If the currentCalculators weight is less than it's top weight, sink the top to its right
                        if(currentCalculator.Weight < topCalculator.Weight)
                        {
                            var tempCurrentCalculator = currentCalculator;
                            var tempTopCalculator = topCalculator;

                            //Loop until the weights have been reordered
                            while (tempCurrentCalculator.Weight < tempTopCalculator.Weight)
                            {
                                //Create a newTopCalculator to replace the current Top calculator
                                //The current Top calculator will sink to the new Top calculators right
                                //and the new Top calculators operation will become the operation of the current calculator
                                var newTopCalculator = new CalculatorCalculation()
                                {
                                    PairCalc = tempCurrentCalculator.PairCalc,
                                    Right = tempTopCalculator,
                                    Top = tempTopCalculator.Top
                                };

                                tempTopCalculator.Left = tempCurrentCalculator.Right;
                                tempTopCalculator.Top = newTopCalculator;
                                //If the current Top calculator was the origin, make the new Top calculator the new origin
                                if (originCalculator == tempTopCalculator)
                                {
                                    originCalculator = newTopCalculator;
                                }

                                //Make the current calculator the newTopCalculator to check if the new Top calculator
                                //has a weight less than the new Top calculators Top.
                                tempCurrentCalculator = newTopCalculator;
                                tempTopCalculator = (CalculatorCalculation)tempCurrentCalculator.Top;

                                if(tempTopCalculator != null)
                                {
                                    if(tempTopCalculator.Weight != -1)
                                    {
                                        tempTopCalculator.Left = newTopCalculator;
                                    }
                                    else
                                    {
                                        tempTopCalculator.Right = newTopCalculator;
                                    }
                                }
                                //If there is no Top calculator break the loop
                                else
                                {
                                    break;
                                }
                            }

                            currentCalculator = tempCurrentCalculator;
                        }
                        else
                        {
                            //If the weights were already ordered, add the currentCalculator to the left of the Top
                            //Don't add to the left if the topCalculator has the weight of a parentheses
                            if (topCalculator.Weight != -1)
                            {
                                topCalculator.Left = currentCalculator;
                            }
                        }
                    }
                    else
                    {
                        //If the current calculator is a OperationNum, add the currentCalculator to the left of the Top
                        //Don't add to the left if the topCalculator has the weight of a parentheses
                        if (topCalculator.Weight != -1)
                        {
                            topCalculator.Left = currentCalculator;
                        }
                    }
                }

                //Setup the currentCalculator as the new Top for the next CalculationCalculator in the loop
                currentCalculator = new CalculatorCalculation()
                {
                    Top =  currentCalculator
                };

            }

            //Run the operations
            return originCalculator;
        }

        /// <summary>
        /// Return the operation Delegate and the weight for the character operation
        /// </summary>
        /// <param name="operation">Operation character</param>
        /// <returns>Calculation pair and operation weight</returns>
        /// <exception cref="System.FormatException">
        /// Thrown when no character matches the character operation
        /// </exception>
        /*private (CalculationPair calculationPair, int weight) GetCalculationPairAndWeight(char operation)
        {
            switch(operation)
            {
                case '+':
                    return (Add, 0);
                case '-':
                    return (Minus, 0);
                case '/':
                    return (Divide, 1);
                case '*':
                    return (Times, 1);
                case '^':
                    return (Exponent, 2);
                case 'r':
                    return (Root, 2);
                //Log case
                case '_':
                    return (Log, 2);
                default:
                    throw new FormatException();
            }
        }*/

        private class CalculationHelper
        {
            public bool FirstDecimal { get; set; }
            public int ParenthesesNum { get; set; }
            public bool OperationNext { get; set; }
            public bool NegativeNum { get; set; }
            public Stack<char> NumberChar { get; set; }
            public Queue<object> OperationOrder { get; set; }

            public CalculationHelper()
            {
                NumberChar = new Stack<char>();
                OperationOrder = new Queue<object>();

                FirstDecimal = true;
                OperationNext = true;
                NegativeNum = false;

                ParenthesesNum = 0;
            }

            public void Reset()
            {
                FirstDecimal = true;
                OperationNext = true;
                NegativeNum = false;
                NumberChar.Clear();
            }
        }
    }
}

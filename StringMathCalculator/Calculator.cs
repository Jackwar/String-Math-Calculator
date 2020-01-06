using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StringMathCalculator.Calculations;

namespace StringMathCalculator
{

    public class Calculator
    {
        ///<value>Regex for removing spaces from a string.</value>
        private readonly Regex removeSpaces = new Regex(@"\t |\n |\r |\s");
        ///<value>Regex for checking a string with calculations for illegal values.</value>
        private readonly Regex legalCharacters = new Regex(@"[^0-9+-/*\^r()_]");

        /*private bool firstDecimal = true;
        private int parenthesesNum = 0;
        private bool operationNext = true;
        private bool negativeNum = false;*/

        /*
          
            Methods for calculations. 
            
            How to add a calculation operation with a single character (example, '*')

            1. Choose a character for the operation.
            2. Add a new method that that takes two doubles as parameters and returns a double.
            3. Add a new switch case to the method GetCalculationPairAndWeight for the chosen character along with 
                its weight compared to other operations. The higher the weight is
                the sooner it will be calculated (example, Times with a weight of 1 will 
                always be calculated before Add with a weight of 0). If calculation weights are equal
                the left operation is always calculated first.

            4. Add the chosen character to the legalCharacter regex.

            How to add a calculation with multiple characters (example, "log")

            1. Choose a single character to replace the multiple characters (example, "log" is replaced by '_')
            2. Add a new operation method that takes two doubles as parameters and returns a double.
            3. Add a new switch case to the method GetCalculationPairAndWeight for the chosen character along with 
                its weight compared to other operations. The higher the weight is
                the sooner it will be calculated (example, Times with a weight of 1 will 
                always be calculated before Add with a weight of 0). If calculation weights are equal
                the left operation is always calculated first.

            4. Add the chosen character to the legalCharacter regex.
            5. Chain a new Replace onto calculations in the method CalculatorCalculation (The first line in the method)
                that replaces the multiple characters with the chosen character
            add a new method that takes two doubles as parameters, then use any unused
            character to replace the calculation name and add it to 
           
        */
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
        public CalculatorCalculation Calculation(string calculations)
        {

            calculations = removeSpaces.Replace(calculations, "").ToLower()
                .Replace("log", "_");

            if(legalCharacters.IsMatch(calculations))
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

                    var (calculationPair, weight) = GetCalculationPairAndWeight(calculations[i]);
                    calculationHelper.OperationOrder.Enqueue(
                        new OperationPair(number,
                                      weight,
                                      calculationPair));
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
                                var (calculationPair, weight) = GetCalculationPairAndWeight('*');
                                OperationParentheses operationParentheses = new OperationParentheses(Parentheses.LEFT)
                                {
                                    calcPair = calculationPair,
                                    Weight = weight
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
                                var (calculationPair, weight) = GetCalculationPairAndWeight(leftCharacter);
                                OperationParentheses operationParentheses = new OperationParentheses(Parentheses.LEFT)
                                {
                                    calcPair = calculationPair,
                                    Weight = weight
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

                var (calculationPair, weight) = GetCalculationPairAndWeight('*');
                calculationHelper.OperationOrder.Enqueue(
                    new OperationPair(number,
                                  weight,
                                  calculationPair));
            }

            calculationHelper.OperationOrder.Enqueue(new OperationParentheses(Parentheses.RIGHT));
        }

        //Reorder the operations in the opertionsOrder Queue by weight of operation
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

        //Return the opeartion Delegate and the weight for the character operation
        //If the character isn't listed throw a FormatException
        private (CalculationPair calculationPair, int weight) GetCalculationPairAndWeight(char operation)
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
        }

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

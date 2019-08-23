using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StringMathCalculator.Calculations;

namespace StringMathCalculator
{

    public class Calculator
    {
        //private Regex regex = new Regex(@"[+-\\*/^rg]", RegexOptions.Compiled);
        private Regex removeSpaces = new Regex(@"\t |\n |\r |\s", RegexOptions.Compiled);
        private Regex legalCharacters = new Regex(@"[^0-9+-/*\^r()]", RegexOptions.Compiled);

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

        //The main method for calculations
        //Reads the calculation string from right to left and parses the characters to doubles and operations 
        //if the string provided has illegal characters, or too many decimals in a number throw a FormatException

        public CalculatorCalculation Calculation(string calculations)
        {

            //calculations = calculations.Trim();

            calculations = removeSpaces.Replace(calculations, "").ToLower();
            string logRemoved = calculations.Replace("log", "");

            if(legalCharacters.IsMatch(logRemoved))
            {
                throw new FormatException("Illegal characters in calculation.");
            }

            bool firstDecimal = true;
            int parenthesesNum = 0;
            bool operationNext = true;
            bool negativeNum = false;
            Stack<char> numberChar = new Stack<char>();
            Queue<object> operationOrder = new Queue<object>();

            for (int i = calculations.Length - 1; i > -1; i--)
            {
                var isNum = int.TryParse(calculations[i].ToString(), out _);

                //If the character is a number add it to the numberChar queue
                if (isNum)
                {
                    operationNext = true;
                    numberChar.Push(calculations[i]);
                    //Check if we are on the end of the operations string
                    //If we are, add a SingleNum to the operation order with the number
                    if (i == 0)
                    {
                        string numberString = new string(numberChar.ToArray());
                        operationOrder.Enqueue(new OperationNum(double.Parse(numberString)));
                    }
                }
                else if (calculations[i].Equals(')'))
                {
                    parenthesesNum++;

                    //Check if there is a number waiting to be passed in numberChar
                    if (numberChar.Count != 0)
                    {
                        if(negativeNum && numberChar.Count == 1)
                        {
                            throw new FormatException("Double negative found in front of parentheses.");
                        }

                        negativeNum = false;
                        operationNext = false;
                        firstDecimal = true;

                        double number = double.Parse(new string(numberChar.ToArray()));
                        numberChar.Clear();

                        var operation = GetCalculationPairAndWeight('*');
                        operationOrder.Enqueue(
                            new OperationPair(number,
                                          operation.weight,
                                          operation.calculationPair));
                    }

                    operationOrder.Enqueue(new OperationParentheses(false, true));
                }
                else if (calculations[i].Equals('('))
                {
                    //Check if we have right parentheses in the string for the left parentheses to close.
                    //Also check if there is a number waiting to be passed.
                    if (parenthesesNum > 0 && numberChar.Count != 0)
                    {

                        firstDecimal = true;
                        negativeNum = false;
                        operationNext = false;

                        //Queue the number that is in numberChar
                        //string numberString = new string(numberChar.ToArray());
                        operationOrder.Enqueue(
                            new OperationNum(
                                double.Parse(new string(numberChar.ToArray()))));
                        numberChar.Clear();

                        //Check if we are at the end of the string or not
                        if (i > 0)
                        {
                            int i2;
                            //Loop for numbers or operations after the left parentheses.
                            for (i2 = i; i2 > 0 && parenthesesNum > 0; i2--)
                            {
                                parenthesesNum--;
                                char leftCharacter = calculations[i2 - 1];

                                if (leftCharacter.Equals('('))
                                {
                                    var operationParentheses = new OperationParentheses(true, false);
                                    operationOrder.Enqueue(operationParentheses);
                                }
                                //Check if there is a number, right parentheses or operation left of the left parentheses
                                else
                                {
                                    bool isLeftNum = int.TryParse(leftCharacter.ToString(), out _);

                                    if (isLeftNum || leftCharacter.Equals(')'))
                                    {
                                        var pairAndWeight = GetCalculationPairAndWeight('*');
                                        OperationParentheses operationParentheses = new OperationParentheses(true, false)
                                        {
                                            calcPair = pairAndWeight.calculationPair,
                                            Weight = pairAndWeight.weight
                                        };
                                        operationOrder.Enqueue(operationParentheses);

                                        break;
                                    }
                                    else if (leftCharacter.Equals('('))
                                    {
                                        var operationParentheses = new OperationParentheses(true, false);
                                        operationOrder.Enqueue(operationParentheses);
                                    }
                                    else
                                    {
                                        var pairAndWeight = GetCalculationPairAndWeight(leftCharacter);
                                        OperationParentheses operationParentheses = new OperationParentheses(true, false)
                                        {
                                            calcPair = pairAndWeight.calculationPair,
                                            Weight = pairAndWeight.weight
                                        };
                                        operationOrder.Enqueue(operationParentheses);

                                        if(leftCharacter.Equals('g'))
                                        {
                                            i2 -= 2;
                                        }

                                        i = i2 - 1;

                                        break;
                                    }
                                }
                            }

                            //Check if the end of the string had closed all parentheses.
                            if (i2 == 0 && parenthesesNum == 1)
                            {
                                var operationParentheses = new OperationParentheses(true, false);
                                operationOrder.Enqueue(operationParentheses);

                                i = 0;
                            }
                            else if (i2 == 0 && parenthesesNum > 0)
                            {
                                throw new FormatException("Parentheses not all closed at start of string.");
                            }
                        }
                        else
                        {
                            var operationParentheses = new OperationParentheses(true, false);
                            operationOrder.Enqueue(operationParentheses);
                        }
                    }
                    else
                    {
                        if(numberChar.Count == 0)
                        {
                            throw new FormatException("Operation found at the end of parentheses");
                        }
                        else
                        {
                            throw new FormatException("Left parentheses found without enclosing right parentheses");
                        }
                    }
                }
                else if (calculations[i].Equals('.'))
                {
                    if(!firstDecimal)
                    {
                        throw new FormatException("Found more than one decimal point in a single number.");
                    }

                    if(numberChar.Count != 0)
                    {
                        if(negativeNum && numberChar.Count == 1)
                        {
                            numberChar.Push('0');
                        }
                        numberChar.Push(calculations[i]);
                        firstDecimal = false;
                    }
                    else
                    {
                        numberChar.Push('0');
                        numberChar.Push(calculations[i]);
                        firstDecimal = false;
                    }
                }
                else if(!operationNext)
                {
                    throw new FormatException("Operation found next to another operation.");
                }
                else
                {
                    if(calculations[i].Equals('-'))
                    {
                        if(i == 0)
                        {
                            numberChar.Push(calculations[i]);

                            operationOrder.Enqueue(
                                new OperationNum(
                                    double.Parse(new string(numberChar.ToArray()))));

                            continue;
                        }
                        else
                        {
                            char leftCharacter = calculations[i - 1];

                            bool isLeftNum = int.TryParse(leftCharacter.ToString(), out _);

                            if(!isLeftNum  
                                && !leftCharacter.Equals(')'))
                            {
                                negativeNum = true;
                                numberChar.Push(calculations[i]);

                                continue;
                            }
                        }
                    }

                    if(i == 0)
                    {
                        throw new FormatException("Operation found at the beginning of the string");
                    }

                    negativeNum = false;
                    operationNext = false;
                    firstDecimal = true;

                    double number = double.Parse(new string(numberChar.ToArray()));
                    numberChar.Clear();

                    var operation = GetCalculationPairAndWeight(calculations[i]);
                    operationOrder.Enqueue(
                        new OperationPair(number,
                                      operation.weight,
                                      operation.calculationPair));

                    if (calculations[i].Equals('g'))
                    {
                        i -= 2;
                    }

                }
            }

            return OrderCalculations(operationOrder);
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
                    if(operationParentheses.RightParentheses)
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

        private OperationPair GetOperationPair(int weight, double number, CalculationPair calculationPair, bool operationNext)
        {
            if(!operationNext)
            {
                throw new FormatException("Operation found next to another operation.");
            }

            return new OperationPair(number, weight, calculationPair);
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
                case 'g':
                    return (Log, 2);
                default:
                    throw new FormatException();
            }
        }
    }
}

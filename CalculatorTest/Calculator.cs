﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculations
{
    public delegate double CalculationPair(double x, double y);
    public delegate double CalculationSingle(double x);

    public class Calculator
    {
        private Regex regex = new Regex(@"[+-\\*/^r]", RegexOptions.Compiled);
        //private delegate double calculationDelegate(double x, double y);

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

        //The main method for calculations
        //Reads the calculation string from right to left and parses the characters to doubles and operations 
        //if the string provided has illegal characters, or too many .'s in a number throw a FormatException
        //
        public double Calculation(string calculations)
        {

            calculations.Trim();
            string calcNoSpaces = calculations.Replace(" ", "");

            bool firstDot = true;
            Stack<char> numberChar = new Stack<char>();
            Queue<Operation> operationOrder = new Queue<Operation>();

            for (int i = calcNoSpaces.Length - 1; i > -1; i--)
            {
                char character = calcNoSpaces[i];

                //Check if the chacter is a .
                if(character.Equals('.'))
                {
                    if(firstDot)
                    {
                        numberChar.Push(character);
                        firstDot = false;
                    }
                    //A number cannot have more than one . in it.
                    //If it does, throw a Format Exception.
                    else
                    {   
                        throw new FormatException();
                    }
                }
                else
                {
                    //Check if the character is a number
                    var isNum = int.TryParse(character.ToString(), out _);

                    //If the character is a number add it to the numberChar queue
                    if (isNum)
                    {
                        numberChar.Push(character);
                        //Check if we are on the end of the operations string
                        //If we are, add a SingleNum to the operation order with the number
                        if(i == 0)
                        {
                            string numberString = new string(numberChar.ToArray());
                            operationOrder.Enqueue(new OperationNum(double.Parse(numberString)));
                        }
                    }
                    else
                    {
                        //The current number in numberChar will be saved
                        //Reset the check for more than one . in a number
                        firstDot = true;
                        double currentNumber = 0;

                        //If there is a number in numberChar, then save the number
                        if (numberChar.Count > 0)
                        {
                            //Check if the number is negative
                            if(character.Equals('-'))
                            {
                                //Check if we are at the end of the string
                                //If we are not, check for a new operation after the '-' character
                                if(i > 0)
                                {
                                    char nextCharacter = calcNoSpaces[i - 1];
                                    isNum = int.TryParse(nextCharacter.ToString(), out _);

                                    if(!isNum)
                                    {
                                        numberChar.Push(character);
                                        character = calcNoSpaces[--i];
                                    }
                                }
                                else
                                {
                                    numberChar.Push(character);
                                }
                            }
                            string numberString = new string(numberChar.ToArray());
                            currentNumber = double.Parse(numberString);
                            numberChar.Clear();
                        }

                        //Check if the non number character is a math operator
                        //If it is, add it to the operation queue
                        //Else throw an exception
                        if(regex.IsMatch(character.ToString()))
                        {
                            var  calculationPairAndWeight = GetCalculationPairAndWeight(character);
                            operationOrder.Enqueue(
                                new OperationPair(currentNumber,
                                                  calculationPairAndWeight.weight,
                                                  calculationPairAndWeight.calculationPair));
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                }

            }

            return OrderCalculations(operationOrder);
        }

        //Reorder the operations in the opeartionsOrder Queue by weight of operation
        private double OrderCalculations(Queue<Operation> operationOrder)
        {
            //The originCalculator is the top of the binary tree structure for the operations order
            var originCalculator = new CalculatorCalculation();
            //The current calculation, start with the origin
            var currentCalculator = originCalculator;

            //Loop through all the opearations and order them according to the order of operations
            foreach(Operation operation in operationOrder)
            {
                //Put the number associated with the operation to the right of the tree as a single number
                var rightCalculator = new CalculatorNumber(operation.X);
                currentCalculator.Right = rightCalculator;
                
                //Check if the Operation is a OperationPair
                if(operation is OperationPair)
                {
                    OperationPair operationPair = (OperationPair)operation;
                    currentCalculator.PairCalc = operationPair.CalcPair;
                    currentCalculator.Weight = operationPair.Weight;
                }
                else if (operation is OperationNum)
                {
                    //If the Operation is an OperationNum, don't assign a calculation
                    //and make the weight -1 so its operation order is never checked
                    currentCalculator.Weight = -1;
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
                                    tempTopCalculator.Left = newTopCalculator;
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
                            topCalculator.Left = currentCalculator;
                        }
                    }
                    else
                    {
                        //If the current calculator is a OperationNum, add the currentCalculator to the left of the Top
                        topCalculator.Left = currentCalculator;
                    }
                }

                //Setup the currentCalculator as the new Top for the next CalculationCalculator in the loop
                var tempCalculator = currentCalculator;
                currentCalculator = new CalculatorCalculation();
                currentCalculator.Top = tempCalculator;

            }

            //Run the operations
            return originCalculator.Calculate();
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
                default:
                    throw new FormatException();
            }
        }
    }
}

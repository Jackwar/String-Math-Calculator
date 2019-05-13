using System;
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
        private Regex regex = new Regex(@"[+-\\*/]", RegexOptions.Compiled);
        //private delegate double calculationDelegate(double x, double y);

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

        private double SquareRoot(double x)
        {
            return Math.Sqrt(x);
        }

        //private void setCalculation(C)

        //The main method for calculations, will throw a FormatException
        //if the string provided by the user has illegal characters, or too many .'s in a number
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
                            var numberBuilder = new StringBuilder();
                            numberBuilder.Append(numberChar.ToArray());
                            operationOrder.Enqueue(new SingleNum(double.Parse(numberBuilder.ToString())));
                        }
                    }
                    else
                    {
                        //The current number in numberChar will be saved
                        //Reset the check for more than one . in a number
                        firstDot = true;
                        double currentNumber = 0;
                        //If there is a number in numberChar, then save
                        //the number
                        if (numberChar.Count > 0)
                        {
                            var numberBuilder = new StringBuilder();
                            numberBuilder.Append(numberChar.ToArray());
                            currentNumber = double.Parse(numberBuilder.ToString());
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

        private double OrderCalculations(Queue<Operation> operationOrder)
        {
            var originCalculator = new CalculatorCalculation();
            var currentCalculator = originCalculator;
            //CalculatorCalculation topCalculator = null;

            foreach(Operation operation in operationOrder)
            {
                var rightCalculator = new CalculatorNumber(operation.X);
                currentCalculator.Right = rightCalculator;
                
                if(operation is OperationPair)
                {
                    OperationPair operationPair = (OperationPair)operation;
                    currentCalculator.PairCalc = operationPair.CalcPair;
                    currentCalculator.Weight = operationPair.Weight;
                }
                else if (operation is SingleNum)
                {
                    currentCalculator.Weight = -1;
                }

                var topCalculator = (CalculatorCalculation)currentCalculator.Top;

                if(topCalculator != null)
                {
                    if(topCalculator.Weight != -1 && currentCalculator.Weight != -1)
                    {
                        if(currentCalculator.Weight < topCalculator.Weight)
                        {
                            var tempCurrentCalculator = currentCalculator;
                            var tempTopCalculator = topCalculator;

                            while (tempCurrentCalculator.Weight < tempTopCalculator.Weight)
                            {
                                var newTopCalculator = new CalculatorCalculation()
                                {
                                    PairCalc = tempCurrentCalculator.PairCalc,
                                    Right = tempTopCalculator,
                                    Top = tempTopCalculator.Top
                                };

                                tempTopCalculator.Left = tempCurrentCalculator.Right;
                                tempTopCalculator.Top = newTopCalculator;
                                if (originCalculator == tempTopCalculator)
                                {
                                    originCalculator = newTopCalculator;
                                }

                                tempCurrentCalculator = newTopCalculator;
                                tempTopCalculator = (CalculatorCalculation)tempCurrentCalculator.Top;

                                if(tempTopCalculator != null)
                                {
                                    tempTopCalculator.Left = newTopCalculator;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            currentCalculator = tempCurrentCalculator;
                        }
                        else
                        {
                            topCalculator.Left = currentCalculator;
                            //topCalculator = currentCalculator;
                        }
                    }
                    else
                    {
                        topCalculator.Left = currentCalculator;
                        //topCalculator = currentCalculator;
                    }
                }
                /*else
                {
                    topCalculator.Left = currentCalculator;
                    topCalculator = currentCalculator;
                }*/

                var tempCalculator = currentCalculator;
                currentCalculator = new CalculatorCalculation();
                currentCalculator.Top = tempCalculator;

            }

            return originCalculator.Calculate();
        }



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
                default:
                    return (Add, 1);
            }
        }
    }
}

using System;
using Calculations;

namespace CalculatorTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Calculator calculator = new Calculator();
            Calculator calculator = new Calculator();
            

            Console.WriteLine(calculator.Calculation("5 - 7 + 9 * 6 / 5 + 6 * 6 + 3 - 6 / 5"));

        }
    }
}

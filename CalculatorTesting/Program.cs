using System;
using Calculations;

namespace CalculatorTesting
{
    //Examples
    class Program
    {
        static void Main(string[] args)
        {
            //Supported operations are +, -, / *
            //Exponents are with the ^ symbol. (Number ^ Power)
            //Roots are done with r. (Number r Root)

            Calculator calculator = new Calculator();
            
            Console.WriteLine(calculator.Calculation("((5 - ((7 + 9) * (6 / 5) * (6 * (6 + 3))) - 6 / 5) + (7 + 5))"));

            //Square root of 4. Any root can be used on the right side of r.
            Console.WriteLine(calculator.Calculation("4r2"));

            //Exponents, 4 by the power of two. 
            Console.WriteLine(calculator.Calculation("4^2"));

            //Order of operations are performed left to right, in PEMDAS order.
            Console.WriteLine(calculator.Calculation("3 + 4 * 2"));

            //Parentheses can be used
            Console.WriteLine(calculator.Calculation("(3 + 4) * 2"));
        }
    }
}

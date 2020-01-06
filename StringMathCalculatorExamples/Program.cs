using System;
using StringMathCalculator;

namespace CalculatorTesting
{
    //Examples
    class Program
    {
        static void Main(string[] args)
        {
            //Supported operations are + - / * ^ r log
            //Exponents are with the ^ symbol. (Number ^ Power)
            //Roots are done with r. (Number r Root)
            //Log is with log. (Number log log)

            Calculator calculator = new Calculator();

            //Square root of 4. Any root can be used on the right side of r.
            Console.WriteLine(calculator.Calculation("4 r 2").Calculate());

            //Exponents, 4 by the power of two. 
            Console.WriteLine(calculator.Calculation("4 ^ 2").Calculate());

            //Log, 4 by the log of 2.
            Console.WriteLine(calculator.Calculation("4 log 2").Calculate());

            //Order of operations are performed left to right, in PEMDAS order.
            Console.WriteLine(calculator.Calculation("3 + 4 * 2").Calculate());

            //Parentheses can be used
            Console.WriteLine(calculator.Calculation("(3 + 4) * 2").Calculate());

            //The times operator can be ommited when next to a parentheses
            Console.WriteLine(calculator.Calculation("(3 + 4) 2").Calculate());

            //This works in both directions
            Console.WriteLine(calculator.Calculation("2 (3 + 4)").Calculate());
        }
    }
}

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

            ////Square root of 4. Any root can be used on the right side of r.
            //Console.WriteLine(calculator.Calculation("4 r 2").Calculate());

            ////Exponents, 4 by the power of two. 
            //Console.WriteLine(calculator.Calculation("4 ^ 2").Calculate());

            ////Log, 4 by the log of 2.
            //Console.WriteLine(calculator.Calculation("4 log 2").Calculate());

            ////Order of operations are performed left to right, in PEMDAS order.
            //Console.WriteLine(calculator.Calculation("3 + 4 * 2").Calculate());

            ////Parentheses can be used
            //Console.WriteLine(calculator.Calculation("(3 + 4) * 2").Calculate());

            ////The times operator can be omitted when next to a parentheses
            //Console.WriteLine(calculator.Calculation("(3 + 4) 2").Calculate());

            ////This works in both directions
            //Console.WriteLine(calculator.Calculation("2 (3 + 4)").Calculate());

            ////Custom operations can be added
            //calculator.AddOperation((x, y) => x + x + y + y, 3, 'f');
            //Console.WriteLine(calculator.Calculation("2 f 2").Calculate());

            ////Custom operations with words can be added.
            ////The word cannot have spaces and must be lowercase.
            //calculator.AddOperation((x, y) => x + x + y, 3, "addtwice");
            //Console.WriteLine(calculator.Calculation("1 addtwice 3").Calculate());

            //calculator.AddOperation((x, y) => x + x + y, 3, "addthreetimes");
            //Console.WriteLine(calculator.Calculation("1 addtwice 3").Calculate());

            //calculator.AddOperation((x, y) => x + x + y, 3, "addthreeimes");
            //Console.WriteLine(calculator.Calculation("1 addtwice 3").Calculate());
            //double total = 0;
            //for (int i = 0; i < 10000; i++)
            //{
            //    /*Console.WriteLine(*/
            //    total += calculator.Calculation("1 addtwice 3").Calculate();//)//;
            //}

            //Console.WriteLine(total);

            calculator.AddOperation((x, y) => 0, 0, "33l");
            calculator.AddOperation((x, y) => x + x + y + y, 0, (char)161);
            Console.WriteLine(calculator.Calculation("333l 3").Calculate());
        }
    }
}

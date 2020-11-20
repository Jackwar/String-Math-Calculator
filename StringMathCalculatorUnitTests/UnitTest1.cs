using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringMathCalculator;
using System;

namespace CalculatorUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private Calculator calculator = new Calculator();

        [TestMethod]
        public void Calculator_Add_TwoNumbers()
        {
            double product = calculator.Calculation("5 + 5").Calculate();

            Assert.AreEqual(10, product);
        }

        [TestMethod]
        public void Calculator_Add_ThreeNumbers()
        {
            double product = calculator.Calculation("5 + 5 + 5").Calculate();

            Assert.AreEqual(15, product);
        }

        [TestMethod]
        public void Calculator_Minus_TwoNumbers()
        {
            double product = calculator.Calculation("5 - 5").Calculate();

            Assert.AreEqual(0, product);
        }

        [TestMethod]
        public void Calculator_Minus_ThreeNumbers()
        {
            double product = calculator.Calculation("5 - 5 - 5").Calculate();

            Assert.AreEqual(-5, product);
        }

        [TestMethod]
        public void Calculator_Times_TwoNumbers()
        {
            double product = calculator.Calculation("5 * 5").Calculate();

            Assert.AreEqual(25, product);
        }

        [TestMethod]
        public void Calculator_Times_ThreeNumbers()
        {
            double product = calculator.Calculation("5 * 5 * 5").Calculate();

            Assert.AreEqual(125, product);
        }

        [TestMethod]
        public void Calculator_Divide_TwoNumbers()
        {
            double product = calculator.Calculation("5 / 5").Calculate();

            Assert.AreEqual(1, product);
        }

        [TestMethod]
        public void Calculator_Divide_ThreeNumbers()
        {
            double product = calculator.Calculation("5 / 5 / 5").Calculate();

            Assert.AreEqual(0.2, product);
        }

        [TestMethod]
        public void Calculator_Exponent_TwoNumbers()
        {
            double product = calculator.Calculation("5 ^ 5").Calculate();

            Assert.AreEqual(3125, product);
        }

        [TestMethod]
        public void Calculator_Exponent_ThreeNumbers()
        {
            double product = calculator.Calculation("5 ^ 5 ^ 2").Calculate();

            Assert.AreEqual(9765625, product);
        }

        [TestMethod]
        public void Calculator_SquareRoot_TwoNumbers()
        {
            double product = calculator.Calculation("16 r 2").Calculate();

            Assert.AreEqual(4, product);
        }

        [TestMethod]
        public void Calculator_SquareRoot_ThreeNumbers()
        {
            double product = calculator.Calculation("16 r 2 r 2").Calculate();

            Assert.AreEqual(2, product);
        }

        [TestMethod]
        public void Calculator_Exponent_Compare_Times_TwoNumbers()
        {
            double productExponent = calculator.Calculation("3 ^ 3").Calculate();
            double productTimes = calculator.Calculation("3 * 3 * 3").Calculate();

            Assert.AreEqual(productExponent, productTimes);
        }

        [TestMethod]
        public void Calculator_Exponent_Compare_Times_ThreeNumbers()
        {
            double productExponent = calculator.Calculation("3 ^ 3 ^ 3").Calculate();
            double productTimes = calculator.Calculation("3 * 3 * 3 * 3 * 3 * 3 * 3 * 3 * 3").Calculate();

            Assert.AreEqual(productExponent, productTimes);
        }

        [TestMethod]
        public void Calculator_Log_TwoNumbers()
        {
            double product = calculator.Calculation("8 log 2").Calculate();

            Assert.AreEqual(product, 3);
        }

        [TestMethod]
        public void Calculator_Log_AgainstParentheses()
        {
            double product = calculator.Calculation("8 log (1 + 1)").Calculate();

            Assert.AreEqual(product, 3);
        }

        [TestMethod]
        public void Calculator_OrderOfOperations()
        {
            double product = calculator.Calculation("5 * 3 + 7 * 7 * 9 / 10").Calculate();

            Assert.AreEqual(product, 59.1);
        }

        [TestMethod]
        public void Calculator_OrderOfOperations2()
        {
            double product = calculator.Calculation("5 - 7 + 9 * 6 / 5 + 6 * 6 + 3 - 6 / 5").Calculate();

            Assert.AreEqual(product, 46.599999999999994);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers()
        {
            double product = calculator.Calculation("5 - -5").Calculate();

            Assert.AreEqual(product, 10);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers2()
        {
            double product = calculator.Calculation("5 + -5").Calculate();

            Assert.AreEqual(product, 0);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers3()
        {
            double product = calculator.Calculation("5 * -5 + - 5 / -5").Calculate();

            Assert.AreEqual(product, -24);
        }


        [TestMethod]
        public void Calculator_Negative_end_of_parenthese()
        {
            double product = calculator.Calculation("(-5 + 5)").Calculate();

            Assert.AreEqual(product, 0);
        }

        [TestMethod]
        public void Calculator_Parentheses()
        {
            double product = calculator.Calculation("(5 - ((7 + 9) * (6 / 5) * (6 * (6 + 3))) - 6 / 5) + (7 + 5)").Calculate();

            Assert.AreEqual(product, -1021);
        }

        [TestMethod]
        public void Calculator_Parentheses_surrounding()
        {
            double product = calculator.Calculation("((5 - ((7 + 9) * (6 / 5) * (6 * (6 + 3))) - 6 / 5) + (7 + 5))").Calculate();

            Assert.AreEqual(product, -1021);
        }

        [TestMethod]
        public void Calculator_Parentheses_TimesRight()
        {
            double product = calculator.Calculation("(3)8 * 2").Calculate();

            Assert.AreEqual(product, 48);
        }

        [TestMethod]
        public void Calculator_Parentheses_TimesLeft()
        {
            double product = calculator.Calculation("3 * 2(8)").Calculate();

            Assert.AreEqual(product, 48);
        }

        [TestMethod]
        public void Custom_Operation()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => x + x + y + y, 0, 'f');

            double product = calculator.Calculation("3 f 3").Calculate();

            Assert.AreEqual(product, 12);
        }

        [TestMethod]
        public void Custom_Operation_Existing_Char()
        {
            Calculator calculator = new Calculator();
            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, '+'));
        }

        [TestMethod]
        public void Custom_Operation_Existing_Added_Char()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => 0, 0, 'f');
            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, 'f'));
        }

        [TestMethod]
        public void Custom_Operation_Word()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => x + x + y + y, 0, "Hello");

            double product = calculator.Calculation("3 Hello 3").Calculate();

            Assert.AreEqual(product, 12);
            //Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, '+'));
        }

        [TestMethod]
        public void Custom_Operation_Existing_Word()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => 0, 0, "Hello");
            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "Hello"));
        }

        [TestMethod]
        public void Custom_Operation_Existing_Words()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => 0, 0, "Hellos");
            calculator.AddOperation((x, y) => 0, 0, "Hello");
            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "Hello"));
        }

        [TestMethod]
        public void Custom_Operation_Existing_Word_Characters()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => x + x + y + y, 0, "Hello");
            calculator.AddOperation((x, y) => 0, 0, (char)161);

            double product = calculator.Calculation("3 Hello 3").Calculate();

            Assert.AreEqual(product, 12);
        }

        [TestMethod]
        public void Custom_Operation_Existing_Word_Multiple_Characters()
        {
            Calculator calculator = new Calculator();
            calculator.AddOperation((x, y) => x + x + y + y, 0, "Hello");
            for (int i = 161; i < 290; i++)
            {
                calculator.AddOperation((x, y) => 0, 0, (char)i);
            }
            
            double product = calculator.Calculation("3 Hello 3").Calculate();

            Assert.AreEqual(product, 12);
        }

        [TestMethod]
        public void Custom_Operation_Reserved_Character_1()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, '('));
        }

        [TestMethod]
        public void Custom_Operation_Reserved_Character_2()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, ')'));
        }

        [TestMethod]
        public void Custom_Operation_Reserved_Character_3()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, '.'));
        }

        [TestMethod]
        public void Custom_Operation_Space_In_Word()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "word "));
        }

        [TestMethod]
        public void Custom_Operation_Tab_In_Word()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "   word"));
        }

        [TestMethod]
        public void Custom_Operation_New_Line_In_Word()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "\nword"));
        }

        [TestMethod]
        public void Custom_Operation_Carriage_Return_In_Word()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "\rword"));
        }

        [TestMethod]
        public void Custom_Operation_Number_Character()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, '3'));
        }

        [TestMethod]
        public void Custom_Operation_Number_Word()
        {
            Calculator calculator = new Calculator();

            Assert.ThrowsException<ArgumentException>(() => calculator.AddOperation((x, y) => 0, 0, "32354"));
        }

        [TestMethod]
        public void Custom_Operation_Number_And_Word()
        {
            Calculator calculator = new Calculator();

            calculator.AddOperation((x, y) => x + x + y + y, 0, "3hello3");

            double product = calculator.Calculation("3 3hello3 3").Calculate();

            Assert.AreEqual(12, product);
        }

        [TestMethod]
        public void Decimal_At_End()
        {
            Calculator calculator = new Calculator();

            double product = calculator.Calculation(".3 + 3").Calculate();

            Assert.AreEqual(3.3, product);
        }
    }
}

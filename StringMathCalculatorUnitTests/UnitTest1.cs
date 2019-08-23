using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringMathCalculator;

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

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculations;

namespace CalculatorUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private Calculator calculator = new Calculator();

        [TestMethod]
        public void Calculator_Add_TwoNumbers()
        {
            double product = calculator.Calculation("5 + 5");

            Assert.AreEqual(10, product);
        }

        [TestMethod]
        public void Calculator_Add_ThreeNumbers()
        {
            double product = calculator.Calculation("5 + 5 + 5");

            Assert.AreEqual(15, product);
        }

        [TestMethod]
        public void Calculator_Minus_TwoNumbers()
        {
            double product = calculator.Calculation("5 - 5");

            Assert.AreEqual(0, product);
        }

        [TestMethod]
        public void Calculator_Minus_ThreeNumbers()
        {
            double product = calculator.Calculation("5 - 5 - 5");

            Assert.AreEqual(-5, product);
        }

        [TestMethod]
        public void Calculator_Times_TwoNumbers()
        {
            double product = calculator.Calculation("5 * 5");

            Assert.AreEqual(25, product);
        }

        [TestMethod]
        public void Calculator_Times_ThreeNumbers()
        {
            double product = calculator.Calculation("5 * 5 * 5");

            Assert.AreEqual(125, product);
        }

        [TestMethod]
        public void Calculator_Divide_TwoNumbers()
        {
            double product = calculator.Calculation("5 / 5");

            Assert.AreEqual(1, product);
        }

        [TestMethod]
        public void Calculator_Divide_ThreeNumbers()
        {
            double product = calculator.Calculation("5 / 5 / 5");

            Assert.AreEqual(0.2, product);
        }

        [TestMethod]
        public void Calculator_Exponent_TwoNumbers()
        {
            double product = calculator.Calculation("5 ^ 5");

            Assert.AreEqual(3125, product);
        }

        [TestMethod]
        public void Calculator_Exponent_ThreeNumbers()
        {
            double product = calculator.Calculation("5 ^ 5 ^ 2");

            Assert.AreEqual(9765625, product);
        }

        [TestMethod]
        public void Calculator_SquareRoot_TwoNumbers()
        {
            double product = calculator.Calculation("16 r 2");

            Assert.AreEqual(4, product);
        }

        [TestMethod]
        public void Calculator_SquareRoot_ThreeNumbers()
        {
            double product = calculator.Calculation("16 r 2 r 2");

            Assert.AreEqual(2, product);
        }

        [TestMethod]
        public void Calculator_Exponent_Compare_Times_TwoNumbers()
        {
            double productExponent = calculator.Calculation("3 ^ 3");
            double productTimes = calculator.Calculation("3 * 3 * 3");

            Assert.AreEqual(productExponent, productTimes);
        }

        [TestMethod]
        public void Calculator_Exponent_Compare_Times_ThreeNumbers()
        {
            double productExponent = calculator.Calculation("3 ^ 3 ^ 3");
            double productTimes = calculator.Calculation("3 * 3 * 3 * 3 * 3 * 3 * 3 * 3 * 3");

            Assert.AreEqual(productExponent, productTimes);
        }

        [TestMethod]
        public void Calculator_OrderOfOperations()
        {
            double product = calculator.Calculation("5 * 3 + 7 * 7 * 9 / 10");

            Assert.AreEqual(product, 59.1);
        }

        [TestMethod]
        public void Calculator_OrderOfOperations2()
        {
            double product = calculator.Calculation("5 - 7 + 9 * 6 / 5 + 6 * 6 + 3 - 6 / 5");

            Assert.AreEqual(product, 46.599999999999994);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers()
        {
            double product = calculator.Calculation("5 - -5");

            Assert.AreEqual(product, 10);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers2()
        {
            double product = calculator.Calculation("5 + -5");

            Assert.AreEqual(product, 0);
        }

        [TestMethod]
        public void Calculator_NegativeNumbers3()
        {
            double product = calculator.Calculation("5 * -5 + - 5 / -5");

            Assert.AreEqual(product, -24);
        }

        [TestMethod]
        public void Calculator_Parentheses()
        {
            double product = calculator.Calculation("(5 - ((7 + 9) * (6 / 5) * (6 * (6 + 3))) - 6 / 5) + (7 + 5)");

            Assert.AreEqual(product, -1021);
        }

        [TestMethod]
        public void Calculator_Parentheses_surrounding()
        {
            double product = calculator.Calculation("((5 - ((7 + 9) * (6 / 5) * (6 * (6 + 3))) - 6 / 5) + (7 + 5))");

            Assert.AreEqual(product, -1021);
        }
    }
}

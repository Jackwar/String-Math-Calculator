namespace StringMathCalculator.Calculations
{
    /// <summary>
    /// Delegate for pair operations.
    /// </summary>
    /// <param name="x">Left number for operation.</param>
    /// <param name="y">Right number for operation.</param>
    /// <returns>Sum of operation.</returns>
    public delegate double CalculationPair(double x, double y);

    ///<summary><para>Holds operations in a binary tree, weighted by orders of operations order.</para>
    ///<para>The first operation added is always to the right, usually as a CalculatorNumber
    ///unless parentheses are used.</para>
    ///</summary>
    public class CalculatorCalculation : ICalculatorItem
    {

        ///<value>Calculator item in the left of the binary tree, left is executed first.</value>
        public ICalculatorItem Left { get; set; }
        ///<value>Calculator item in the right of the binary tree, right is executed last.</value>
        public ICalculatorItem Right { get; set; }
        ///<value>Calculator item that is the parent of the left and right items.</value>
        public ICalculatorItem Top { get; set; }
        ///<value>Delegate that performs the passed operation for a pair of numbers.</value>
        public CalculationPair PairCalc { get; set; }
        ///<value>The weight of the operations for this calculation, higher weight is executed sooner.</value>
        public int Weight { get; set; }

        public CalculatorCalculation(CalculationPair calculation, int weight)
        {
            PairCalc = calculation;
            Weight = weight;
        }

        public CalculatorCalculation() { }

        public double Calculate()
        {
            if (Left != null && Right != null)
            {
                return PairCalc(Left.Calculate(), Right.Calculate());
            }
            else if (Right != null)
            {
                return Right.Calculate();
            }
            else
            {
                return Left.Calculate();
            }

        }

    }
}

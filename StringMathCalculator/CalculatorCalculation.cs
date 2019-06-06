namespace StringMathCalculator.Calculations
{
    /*
     * 
     * Holds operations in a binary tree, weighted by orders of operations order.
     * The first operation added is always to the right, usually as a CalculatorNumber
     * unless parentheses are used.
     * A single operation can be held.
     * 
     */

    public delegate double CalculationPair(double x, double y);
    public delegate double CalculationSingle(double x);

    public class CalculatorCalculation : ICalculatorItem
    {

        public ICalculatorItem Left { get; set; }
        public ICalculatorItem Right { get; set; }
        public ICalculatorItem Top { get; set; }
        public CalculationPair PairCalc { get; set; }
        public CalculationSingle SingleCalc { get; set; }
        public int Weight { get; set; }
        public bool SingleOperation { get; set; }

        public CalculatorCalculation(CalculationPair calculation, int weight)
        {
            PairCalc = calculation;
            Weight = weight;
        }

        public CalculatorCalculation() { }

        public double Calculate()
        {
            if (PairCalc != null)
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
            else if (SingleCalc != null)
            {
                return SingleCalc(Right.Calculate());
            }
            else
            {
                return Right.Calculate();
            }
        }

    }
}

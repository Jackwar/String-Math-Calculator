namespace StringMathCalculator.Calculations
{
    /// <summary>
    /// A stored Operation that requires a single number to calculate.
    /// </summary>
    class OperationSingle : OperationCalc
    {
        public int Weight { get; private set; }
        public CalculationSingle CalcSingle { get; private set; }       

        public OperationSingle(double x, int weight, CalculationSingle calcSingle) :
            base(x)
        {
            Weight = weight;
            CalcSingle = calcSingle;
        }
    }
}

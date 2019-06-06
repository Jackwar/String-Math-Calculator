namespace StringMathCalculator.Calculations
{
    abstract class OperationCalc
    {
        public double X { get; private set; }

        public OperationCalc(double x)
        {
            X = x;
        }
    }
}

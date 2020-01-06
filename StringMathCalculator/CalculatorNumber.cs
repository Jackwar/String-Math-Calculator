namespace StringMathCalculator.Calculations
{
    ///<summary>
    ///A single number to be held in a CalculatorCalculation's binary tree
    ///</summary>

    public class CalculatorNumber : ICalculatorItem
    {
        private readonly double _num;

        public CalculatorNumber(double num)
        {
            _num = num;
        }

        public double Calculate()
        {
            return _num;
        }
    }
}

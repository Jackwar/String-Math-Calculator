namespace StringMathCalculator.Calculations
{
    class OperationParentheses
    {
        public bool LeftParentheses { get; private set; }
        public bool RightParentheses { get; private set; }
        public int Weight { get; set; }
        public CalculationPair calcPair { get; set; }

        public OperationParentheses(bool leftParentheses, bool rightParentheses)
        {
            LeftParentheses = leftParentheses;
            RightParentheses = rightParentheses;
        }
    }
}

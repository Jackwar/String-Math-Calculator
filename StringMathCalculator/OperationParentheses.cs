namespace StringMathCalculator.Calculations
{
    enum Parentheses
    {
        LEFT,
        RIGHT
    }
    /// <summary>
    /// Parentheses to hold a CalculationPair to complete the operations within
    /// before leaving the parentheses.
    /// </summary>
    class OperationParentheses
    {
        public Parentheses Parentheses { get; }
        public int Weight { get; set; }
        public CalculationPair calcPair { get; set; }

        public OperationParentheses(Parentheses parentheses)
        {
            Parentheses = parentheses;
        }
    }
}

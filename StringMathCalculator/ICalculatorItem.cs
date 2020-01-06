namespace StringMathCalculator.Calculations
{
    ///<summary>
    /// Interface for items that can be held in a CalculatorCalculation 
    /// within a binary tree with a CalculatorCalculation at the top.
    /// <para>CalculatorCalculation is also an ICalculatorItem</para>
    ///</summary>
    public interface ICalculatorItem
    {
        double Calculate();
    }
}

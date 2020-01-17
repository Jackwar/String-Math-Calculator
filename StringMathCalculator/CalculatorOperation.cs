
using System;
using System.Collections.Generic;
using System.Text;

namespace StringMathCalculator.Calculations
{
    /// <summary>
    /// Holds the weight and CalculationPair for an operation character.
    /// </summary>
    struct CalculatorOperation
    {
        /// <value>Calculation pair to be tied with a character.</value>
        public CalculationPair CalculationPair { get; }
        /// <value>The weight of this operation compared to other operations.</value>
        public int Weight { get; }
        /// <value>Optional string for when a character must be escaped to be used in a regex.</value>
        public string RegexCharacter { get; }
        /// <summary>Holds the weight and CalculationPair for an operation.</summary>
        /// <param name="calculationPair">Method that takes two doubles and outputs a double.</param>
        /// <param name="weight">Weight of this operation.</param>
        public CalculatorOperation(CalculationPair calculationPair, int weight)
        {
            CalculationPair = calculationPair;
            Weight = weight;
            RegexCharacter = null;
        }
        /// <summary>
        /// Holds the weight and CalculationPair for an operation.
        /// <para>Also take a string for when a character must be escaped to be used in a regex.</para>
        /// </summary>
        /// <param name="calculationPair">Method that takes two doubles and outputs a double.</param>
        /// <param name="weight">Weight of this operation.</param>
        /// <param name="regexCharacter">String for when a character must be escaped to be used in a regex.</param>
        public CalculatorOperation(CalculationPair calculationPair, int weight, string regexCharacter)
        {
            CalculationPair = calculationPair;
            Weight = weight;
            RegexCharacter = regexCharacter;
        }
    }
}

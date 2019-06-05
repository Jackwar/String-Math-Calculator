using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    class OperationParentheses : IOperation
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

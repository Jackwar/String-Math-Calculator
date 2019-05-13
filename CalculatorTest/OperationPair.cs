using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    class OperationPair : Operation
    {
        public int Weight { get; private set; }
        public CalculationPair CalcPair { get; private set; }
        
        public OperationPair(double x, int weight, CalculationPair calcPair) :
            base(x)
        {
            Weight = weight;
            CalcPair = calcPair;
        }
    }
}

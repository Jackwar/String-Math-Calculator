using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    class OperationSingle : Operation
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    class SingleOperation : Operation
    {
        public int Weight { get; private set; }
        public CalculationSingle CalcSingle { get; private set; }       

        public SingleOperation(double x, int weight, CalculationSingle calcSingle) :
            base(x)
        {
            Weight = weight;
            CalcSingle = calcSingle;
        }
    }
}

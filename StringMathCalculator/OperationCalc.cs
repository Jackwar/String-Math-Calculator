using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
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

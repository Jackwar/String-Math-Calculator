using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    abstract class Operation
    {
        public double X { get; private set; }

        public Operation(double x)
        {
            X = x;
        }
    }
}

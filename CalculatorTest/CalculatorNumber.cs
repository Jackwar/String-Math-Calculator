using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculations
{
    public class CalculatorNumber : CalculatorItem
    {
        private readonly double _num;

        public CalculatorNumber(double num)
        {
            _num = num;
        }

        public double Calculate()
        {
            return _num;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    /*
     * 
     * Single number stored from a calculations string to be passed into
     * a CalculatorItem.
     * 
     */
    class OperationNum : OperationCalc
    {
        public OperationNum(double x) :
            base(x){}
    }
}

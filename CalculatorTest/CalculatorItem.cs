using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculations
{
    /*
     * 
     * Interface for items that can be held in a CalculatorCalculation 
     * for a binary tree with a CalculatorCalculation at the top.
     * 
     */
    public interface CalculatorItem
    {
        double Calculate();
    }
}

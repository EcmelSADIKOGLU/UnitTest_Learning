using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.App;

public class CalculatorService: ICalculatorService
{
    public int Sum(int a, int b)
    {
        return a + b;
    }
}

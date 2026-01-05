using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extentions.Numerics
{
    public static class NumericExtensions
    {
        public static decimal To2DP(this decimal value) =>
           Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}

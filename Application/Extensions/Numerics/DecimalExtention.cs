using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceHelper.Application.Extensions.Numerics
{
    public static class NumericExtensions
    {
        extension(decimal value)
        {
            public decimal To2Dp() =>
                Math.Round(value, 2, MidpointRounding.AwayFromZero);

            public decimal YearlyToMonthly() =>
                Math.Round(value / 12, 2, MidpointRounding.AwayFromZero);

            public decimal YearlyToWeekly() =>
                Math.Round((value / 12) / 4m, 2, MidpointRounding.AwayFromZero);

            public decimal MonthlyToYearly() =>
                Math.Round(value * 12, 2, MidpointRounding.AwayFromZero);

            public decimal MonthlyToWeekly() =>
                Math.Round(value / 4, MidpointRounding.AwayFromZero);

            public decimal WeeklyToYearly() =>
                Math.Round(value * 4 * 12, 2, MidpointRounding.AwayFromZero);

            public decimal WeeklyToMonthly() =>
                Math.Round(value * 4, 2, MidpointRounding.AwayFromZero);
        }
    }
}

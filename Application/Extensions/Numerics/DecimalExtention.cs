using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions.Numerics
{
    public static class NumericExtensions
    {
        public static decimal To2DP(this decimal value) =>
           Math.Round(value, 2, MidpointRounding.AwayFromZero);

        public static decimal YearlyToMonthly(this decimal yearlyValue) =>
            Math.Round(yearlyValue / 12, 2, MidpointRounding.AwayFromZero);
        public static decimal YearlyToWeekly(this decimal yearlyValue) =>
            Math.Round((yearlyValue / 12) / 4m, 2, MidpointRounding.AwayFromZero);
        public static decimal MonthlyToYearly(this decimal monthlyValue) =>
            Math.Round(monthlyValue * 12, 2, MidpointRounding.AwayFromZero);
        public static decimal MonthlyToWeekly(this decimal monthlyValue) =>
            Math.Round(monthlyValue / 4, MidpointRounding.AwayFromZero);

        public static decimal WeeklyToYearly(this decimal weeklyValue) =>
            Math.Round(weeklyValue * 4 * 12, 2, MidpointRounding.AwayFromZero);
        public static decimal WeeklyToMonthly(this decimal weeklyValue) =>
            Math.Round(weeklyValue * 4, 2, MidpointRounding.AwayFromZero);
    }
}

using Application.Enums.Finance;
using Application.Extentions.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Domain.Finance
{
    public class Salary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public int PensionPercentage { get; set; }
        public decimal PensionContribution { get; set; }
        public decimal Tax { get; set; }
        public TaxBand TaxBand { get; set; }
        public decimal NationalInsurance { get; set; }
        public decimal TaxableBenefits { get; set; }
        public bool PayNationalInsurance { get; set; } = true;
        public bool HasStudentLoan { get; set; }
        public StudentPlanType? StudentPlanType { get; set; } = null;
        public decimal StudentLoan { get; set; }
    }

    public class MonthlyTotals
    {
        private MonthlyTotals() { }
        public MonthlyTotals(Salary salary)
        {
            GrossSalary = salary.GrossSalary.YearlyToMonthly();
            NetSalary = salary.NetSalary.YearlyToMonthly();
            PensionContribution = salary.PensionContribution.YearlyToMonthly();
            Tax = salary.Tax.YearlyToMonthly();
            NationalInsurance = salary.NationalInsurance.YearlyToMonthly();
            StudentLoan = salary.StudentLoan.YearlyToMonthly();
        }

        public decimal GrossSalary { get; private set; }
        public decimal NetSalary { get; private set; }
        public decimal PensionContribution { get; private set; }
        public decimal Tax { get; private set; }
        public decimal NationalInsurance { get; private set; }
        public decimal StudentLoan { get; private set; }
    }
}

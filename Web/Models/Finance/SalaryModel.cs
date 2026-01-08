using Application.Domain.Finance;
using Application.Enums.Finance;

namespace Web.Models.Finance
{
    public class SalaryModel
    {
        // Constructor
        public SalaryModel(Salary salary)
        {
            Id = salary.Id;
            GrossSalary = salary.GrossSalary;
            PensionPercentage = salary.PensionPercentage;
            HasStudentLoan = salary.HasStudentLoan;
            StudentPlanType = HasStudentLoan ? salary.StudentPlanType : null;
            PensionContribution = salary.PensionContribution;
            Tax = salary.Tax;
            PayNationalInsurance = salary.PayNationalInsurance;
            NationalInsurance = PayNationalInsurance ? salary.NationalInsurance : 0;
            StudentLoan = HasStudentLoan ? salary.StudentLoan : 0;
            NetSalary = salary.NetSalary;
        }

        public SalaryModel() { }


        public int Id { get; set; }
        public decimal GrossSalary { get; set; }
        public int PensionPercentage { get; set; }
        public bool HasStudentLoan { get; set; }
        public StudentPlanType? StudentPlanType { get; set; }
        public bool PayNationalInsurance { get; set; }
        public decimal Tax { get; set; }
        public decimal NationalInsurance { get; set; }
        public decimal PensionContribution { get; set; }
        public decimal StudentLoan { get; set; }
        public decimal NetSalary { get; set; }

        public SalaryBreakdownModel SalaryMonthly => new SalaryBreakdownModel(this, 12);
        public SalaryBreakdownModel SalaryWeekly => new SalaryBreakdownModel(this, 52);
    }

    public class SalaryBreakdownModel
    {
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public decimal Tax { get; set; }
        public decimal NationalInsurance { get; set; }
        public decimal PensionContribution { get; set; }
        public decimal StudentLoan { get; set; }

        public SalaryBreakdownModel() { }

        public SalaryBreakdownModel(SalaryModel salary, int periods)
        {
            GrossSalary = Decimal.Round(salary.GrossSalary / periods, 2);
            NetSalary = Decimal.Round(salary.NetSalary / periods, 2);
            Tax = Decimal.Round(salary.Tax / periods, 2);
            NationalInsurance = Decimal.Round(salary.NationalInsurance / periods, 2);
            PensionContribution = Decimal.Round(salary.PensionContribution / periods, 2);
            StudentLoan = Decimal.Round(salary.StudentLoan / periods, 2);
        }
    }
}

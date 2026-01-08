using Application.Domain.Finance;
using Application.Enums.Finance;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models.Finance
{
    public class AddSalaryFormModel
    {
        // User Entered
        public int Id { get; set; }
        public decimal GrossSalary { get; set; } = 0;
        public int PensionPercentage { get; set; } = 0;
        public bool PayNationalInsurance { get; set; } = true;
        public bool HasStudentLoan { get; set; } = false;
        public StudentPlanType? StudentPlanType { get; set; } = null;
        public IEnumerable<SelectListItem> StudentPlanTypeOptions { get; set; } = Enumerable.Empty<SelectListItem>();

        // Calculated
        public decimal NetSalary { get; set; } = 0;
        public decimal PensionContribution { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public TaxBand TaxBand { get; set; } = default;
        public decimal NationalInsurance { get; set; } = 0;
        public decimal TaxableBenefits { get; set; } = 0;
        public decimal StudentLoan { get; set; } = 0;

        // Monthly
        public decimal GrossSalaryMonthly { get; set; } = 0;
        public decimal NetSalaryMonthly { get; set; } = 0;
        public decimal PensionContributionMonthly { get; set; } = 0;
        public decimal TaxMonthly { get; set; } = 0;
        public decimal NationalInsuranceMonthly { get; set; } = 0;
        public decimal StudentLoanMonthly { get; set; } = 0;

        public bool IsReview { get; set; } = false;
        public List<String> Errors { get; set; } = new List<string>();

        public void UpdateMonthlyTotals(MonthlyTotals monthlyTotals)
        {
            NetSalaryMonthly = monthlyTotals?.NetSalary ?? 0;
            GrossSalaryMonthly = monthlyTotals?.GrossSalary ?? 0;
            TaxMonthly = monthlyTotals?.Tax ?? 0;
            StudentLoanMonthly = monthlyTotals?.StudentLoan ?? 0;
            PensionContributionMonthly = monthlyTotals?.PensionContribution ?? 0;
            NationalInsuranceMonthly = monthlyTotals?.NationalInsurance ?? 0;
        }
    }
}

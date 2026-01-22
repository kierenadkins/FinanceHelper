using FinanceHelper.Application.Extensions.Numerics;
using FinanceHelper.Application.Services.Tax;
using FinanceHelper.Domain.Enums.Finance;
using FinanceHelper.Domain.Objects.Finance;

namespace FinanceHelper.Application.Services.Salarys;

public class SalaryCalculatorService(ITaxService taxService) : ISalaryCalculatorService
{
    public Salary Calculate(
        decimal grossSalary,
        bool payNationalInsurance,
        bool hasStudentLoan,
        int pensionPercentage,
        StudentPlanType? studentPlanType)
    {
        if (hasStudentLoan && studentPlanType == null)
            throw new InvalidOperationException("StudentPlanType must be set when HasStudentLoan is true");

        var salary = new Salary
        {
            GrossSalary = grossSalary,
            PayNationalInsurance = payNationalInsurance,
            HasStudentLoan = hasStudentLoan,
            StudentPlanType = studentPlanType,
            PensionPercentage = pensionPercentage,
            TaxBand = taxService.GetTaxBand(grossSalary),
            Tax = taxService.CalculateTax(grossSalary).To2DP(),
            NationalInsurance = payNationalInsurance ? taxService.CalculateNationalInsurance(grossSalary).To2DP() : 0,
            StudentLoan = hasStudentLoan ? taxService.CalculateStudentLoan(grossSalary, studentPlanType!.Value).To2DP() : 0,
            PensionContribution = pensionPercentage > 0 ? taxService.CalculatePension(grossSalary, pensionPercentage).To2DP() : 0,
        };

        salary.NetSalary = grossSalary -
                           (salary.Tax + salary.NationalInsurance + salary.PensionContribution + salary.StudentLoan)
                           .To2DP();

        return salary;
    }
}
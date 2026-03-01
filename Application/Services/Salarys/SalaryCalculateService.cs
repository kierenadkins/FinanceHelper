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
        if (grossSalary <= 0)
            throw new ArgumentException("Gross salary must be greater than zero", nameof(grossSalary));

        if (hasStudentLoan && studentPlanType == null)
            throw new InvalidOperationException("StudentPlanType must be set when HasStudentLoan is true");

        if (pensionPercentage < 0 || pensionPercentage > 100)
            throw new ArgumentException("Pension percentage must be between 0 and 100", nameof(pensionPercentage));

        var salary = new Salary
        {
            GrossSalary = grossSalary,
            PayNationalInsurance = payNationalInsurance,
            HasStudentLoan = hasStudentLoan,
            StudentPlanType = studentPlanType,
            PensionPercentage = pensionPercentage,
            TaxBand = taxService.GetTaxBand(grossSalary),
            Tax = taxService.CalculateTax(grossSalary).To2Dp(),
            NationalInsurance = payNationalInsurance ? taxService.CalculateNationalInsurance(grossSalary).To2Dp() : 0,
            StudentLoan = hasStudentLoan ? taxService.CalculateStudentLoan(grossSalary, studentPlanType!.Value).To2Dp() : 0,
            PensionContribution = pensionPercentage > 0 ? taxService.CalculatePension(grossSalary, pensionPercentage).To2Dp() : 0,
        };

        salary.NetSalary = (grossSalary -
                           (salary.Tax + salary.NationalInsurance + salary.PensionContribution + salary.StudentLoan))
                           .To2Dp();

        return salary;
    }
}
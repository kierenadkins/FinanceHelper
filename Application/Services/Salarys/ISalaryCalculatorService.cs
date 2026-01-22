using FinanceHelper.Domain.Enums.Finance;

namespace FinanceHelper.Application.Services.Salarys;

public interface ISalaryCalculatorService
{
    FinanceHelper.Domain.Objects.Finance.Salary Calculate(
        decimal grossSalary,
        bool payNationalInsurance,
        bool hasStudentLoan,
        int pensionPercentage,
        StudentPlanType? studentPlanType);
}
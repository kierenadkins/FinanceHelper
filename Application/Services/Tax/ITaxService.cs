using Application.Enums.Finance;

namespace Application.Services.Tax
{
    public interface ITaxService
    {
        decimal CalculateNationalInsurance(decimal yearlyGross);
        decimal CalculatePension(decimal yearlyGross, int pensionContributionPercentage);
        decimal CalculateStudentLoan(decimal yearlyGross, StudentPlanType studentType);
        decimal CalculateTax(decimal yearlyGross);
        TaxBand GetTaxBand(decimal yearlyGross);
    }
}
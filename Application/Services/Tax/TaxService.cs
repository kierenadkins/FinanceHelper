using Application.Enums.Finance;
using Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Tax
{
    public class TaxService : ITaxService
    {
        private readonly TaxSettings _taxSettings;

        public TaxService(IOptions<TaxSettings> options)
        {
            _taxSettings = options.Value;
        }

        public TaxBand GetTaxBand(decimal yearlyGross)
        {
            if (yearlyGross <= _taxSettings.PersonalAllowance)
                return TaxBand.PersonalAllowance;
            else if (yearlyGross <= _taxSettings.BasicRateLimit)
                return TaxBand.BasicRate;
            else if (yearlyGross <= _taxSettings.HigherRateLimit)
                return TaxBand.HigherRate;
            else
                return TaxBand.AdditionalRate;
        }

        public decimal CalculateTax(decimal yearlyGross)
        {
            decimal totalTax = 0;
            if (yearlyGross <= _taxSettings.PersonalAllowance)
            {
                return totalTax;
            }

            decimal taxableIncome = yearlyGross;

            if (yearlyGross > 100000)
            {
                var allowanceReduction = (yearlyGross - 100000) / 2;
                decimal adjustedAllowance = Math.Max(_taxSettings.PersonalAllowance - allowanceReduction, 0);
                taxableIncome = yearlyGross - adjustedAllowance;
            }
            else
            {
                taxableIncome = yearlyGross - _taxSettings.PersonalAllowance;
            }

            if (taxableIncome > 0)
            {
                decimal basicBandTaxable = Math.Min(taxableIncome, _taxSettings.BasicRateLimit - _taxSettings.PersonalAllowance);
                totalTax += basicBandTaxable * _taxSettings.BasicRate;
                taxableIncome -= basicBandTaxable;
            }

            if (taxableIncome > 0)
            {
                decimal higherBandTaxable = Math.Min(taxableIncome, _taxSettings.HigherRateLimit - _taxSettings.BasicRateLimit);
                totalTax += higherBandTaxable * _taxSettings.HigherRate;
                taxableIncome -= higherBandTaxable;
            }

            if (taxableIncome > 0)
            {
                totalTax += taxableIncome * _taxSettings.AdditionalRate;
            }

            return totalTax;
        }

        public decimal CalculateNationalInsurance(decimal yearlyGross)
        {
            decimal totalNationalInsurance = 0;

            if (yearlyGross <= _taxSettings.NIPrimaryThreshold)
                return totalNationalInsurance;

            decimal remainingIncome = yearlyGross - _taxSettings.NIPrimaryThreshold;

            decimal mainRateIncome = Math.Min(remainingIncome, _taxSettings.NIUpperEarningsLimit - _taxSettings.NIPrimaryThreshold);
            totalNationalInsurance += mainRateIncome * _taxSettings.NIMainRate;
            remainingIncome -= mainRateIncome;

            if (remainingIncome > 0)
            {
                totalNationalInsurance += remainingIncome * _taxSettings.NIUpperRate;
            }

            return totalNationalInsurance;
        }

        public decimal CalculatePension(decimal yearlyGross, int pensionContributionPercentage)
        {
            decimal grossAboveLimit = yearlyGross - _taxSettings.PensionThreshold;

            if (grossAboveLimit <= 0)
                grossAboveLimit = 0;

            decimal pensionContribution = grossAboveLimit * (pensionContributionPercentage / 100m);

            decimal taxFreeContribution = pensionContribution * 0.8m;

            return taxFreeContribution;
        }

        public decimal CalculateStudentLoan(decimal yearlyGross, StudentPlanType studentType)
        {
            decimal studentLoanRepayment = 0m;

            switch (studentType)
            {
                case StudentPlanType.Plan1:
                    if (yearlyGross > _taxSettings.StudentLoan1Threshold)
                    {
                        studentLoanRepayment = (yearlyGross - _taxSettings.StudentLoan1Threshold) * _taxSettings.StudentLoanRate;
                    }
                    break;

                case StudentPlanType.Plan2:
                    if (yearlyGross > _taxSettings.StudentLoan2Threshold)
                    {
                        studentLoanRepayment = (yearlyGross - _taxSettings.StudentLoan2Threshold) * _taxSettings.StudentLoanRate;
                    }
                    break;

                case StudentPlanType.Plan4:
                    if (yearlyGross > _taxSettings.StudentLoan4Threshold)
                    {
                        studentLoanRepayment = (yearlyGross - _taxSettings.StudentLoan4Threshold) * _taxSettings.StudentLoanRate;
                    }
                    break;

                case StudentPlanType.Plan5:
                    if (yearlyGross > _taxSettings.StudentLoan5Threshold)
                    {
                        studentLoanRepayment = (yearlyGross - _taxSettings.StudentLoan5Threshold) * _taxSettings.StudentLoanRate;
                    }
                    break;
                default:
                    studentLoanRepayment = 0m;
                    break;
            }

            return studentLoanRepayment;
        }
    }
}

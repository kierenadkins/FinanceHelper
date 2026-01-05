using Application.Domain.Base;
using Application.Domain.Finance;
using Application.Domain.Users;
using Application.Extentions.Numerics;
using Application.Services.Tax;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Usecases.Finance.Salarys.Command
{
    public class CalculateSalaryTaxablesCommand : IRequest<BaseResult<Salary>>
    {
        public Salary Salary { get; set; }
    }

    public class CalculateSalaryTaxablesHandler : IRequestHandler<CalculateSalaryTaxablesCommand, BaseResult<Salary>>
    {
        private readonly ITaxService _taxService;

        public CalculateSalaryTaxablesHandler(ITaxService taxService)
        {
            _taxService = taxService;
        }

        public async Task<BaseResult<Salary>> Handle(CalculateSalaryTaxablesCommand request, CancellationToken cancellationToken)
        {
            if (request?.Salary == null)
                return BaseResult<Salary>.ErrorResult("User not found");

            var salary = request.Salary;
            var gross = salary.GrossSalary;

            salary.TaxBand = _taxService.GetTaxBand(gross);
            salary.Tax = _taxService.CalculateTax(gross).To2DP();

            if (salary.PayNationalInsurance)
            {
                salary.NationalInsurance = _taxService.CalculateNationalInsurance(gross).To2DP();
            }

            if (salary.HasStudentLoan)
            {
                var planType = salary.StudentPlanType
                    ?? throw new InvalidOperationException("StudentPlanType must be set when HasStudentLoan is true");

                salary.StudentLoan = _taxService.CalculateStudentLoan(gross, planType).To2DP();
            }

            if (salary.PensionPercentage > 0)
            {
                salary.PensionContribution = _taxService.CalculatePension(gross, salary.PensionPercentage).To2DP();
            }

            salary.NetSalary = salary.GrossSalary - (salary.Tax + salary.NationalInsurance + salary.PensionContribution + salary.StudentLoan).To2DP();
            return BaseResult<Salary>.SuccessResult(salary);
        }
    }
}

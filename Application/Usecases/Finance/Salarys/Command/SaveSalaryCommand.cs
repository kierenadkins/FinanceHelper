using FinanceHelper.Application.Services.Finance;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using FinanceHelper.Domain.Enums.Finance;
using FluentValidation;

namespace FinanceHelper.Application.Usecases.Finance.Salarys.Command
{
    public class SaveSalaryCommand : IRequest<BaseResult>
    {
        public decimal GrossSalary { get; set; }
        public bool HasStudentLoan { get; set; }
        public decimal NationalInsurance { get; set; }
        public decimal NetSalary { get; set; }
        public bool PayNationalInsurance { get; set; }
        public decimal PensionContribution { get; set; }
        public int PensionPercentage { get; set; }
        public decimal StudentLoan { get; set; }
        public StudentPlanType? StudentPlanType { get; set; }
        public decimal Tax { get; set; }
        public TaxBand TaxBand { get; set; }
    }

    public class SaveSalaryCommandHandler : IRequestHandler<SaveSalaryCommand, BaseResult>
    {
        private readonly ISalaryService _salaryService;
        private readonly IUserAccountService _userAccountService;

        public SaveSalaryCommandHandler(ISalaryService salaryService, IUserAccountService userAccountService)
        {
            _salaryService = salaryService;
            _userAccountService = userAccountService;
        }

        public async Task<BaseResult> Handle(SaveSalaryCommand request, CancellationToken cancellationToken)
        {
            var result = new BaseResult();

            var userId = _userAccountService.GetCurrent();

            if (userId == 0)
            {
                result.AddError("User not authenticated.");
                return result;
            }

            var salary = new Salary
            {
                GrossSalary = request.GrossSalary,
                HasStudentLoan = request.HasStudentLoan,
                NationalInsurance = request.NationalInsurance,
                NetSalary = request.NetSalary,
                PayNationalInsurance = request.PayNationalInsurance,
                PensionContribution = request.PensionContribution,
                PensionPercentage = request.PensionPercentage,
                StudentLoan = request.StudentLoan,
                StudentPlanType = request.StudentPlanType, 
                Tax = request.Tax,
                TaxBand = request.TaxBand,
                UserId = userId
            };

            await _salaryService.AddAsync(salary);
            return result;
        }

        public class SaveSalaryCommandValidator : AbstractValidator<SaveSalaryCommand>
        {
            public SaveSalaryCommandValidator()
            {
                RuleFor(x => x.GrossSalary)
                    .GreaterThan(0).WithMessage("Gross salary must be greater than 0.");

                RuleFor(x => x.NetSalary)
                    .GreaterThanOrEqualTo(0).WithMessage("Net salary cannot be negative.")
                    .LessThanOrEqualTo(x => x.GrossSalary).WithMessage("Net salary cannot exceed gross salary.");

                RuleFor(x => x.PensionContribution)
                    .GreaterThanOrEqualTo(0).WithMessage("Pension contribution cannot be negative.");

                RuleFor(x => x.PensionPercentage)
                    .InclusiveBetween(0, 100).WithMessage("Pension percentage must be between 0 and 100.");

                RuleFor(x => x.StudentLoan)
                    .GreaterThanOrEqualTo(0).WithMessage("Student loan cannot be negative.");

                RuleFor(x => x.Tax)
                    .GreaterThanOrEqualTo(0).WithMessage("Tax cannot be negative.");

                RuleFor(x => x.TaxBand)
                    .IsInEnum().WithMessage("Invalid tax band.");

                RuleFor(x => x.NationalInsurance)
                    .GreaterThanOrEqualTo(0).WithMessage("National Insurance cannot be negative.");
            }
        }

}
}

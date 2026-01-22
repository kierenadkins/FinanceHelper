using FinanceHelper.Application.Services.Salarys;
using FinanceHelper.Domain.Enums.Finance;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Finance.Salarys.Command
{
    public class CalculateSalaryTaxablesCommand : IRequest<BaseResult<Salary>>
    {
        public decimal GrossSalary { get; set; }
        public bool PayNationalInsurance { get; set; }
        public bool HasStudentLoan { get; set; }
        public StudentPlanType? StudentPlanType { get; set; } 
        public int PensionPercentage { get; set; }
    }

    public class CalculateSalaryTaxablesHandler : IRequestHandler<CalculateSalaryTaxablesCommand, BaseResult<Salary>>
    {
        private readonly ISalaryCalculatorService _calculator;

        public CalculateSalaryTaxablesHandler(ISalaryCalculatorService calculator)
        {
            _calculator = calculator;
        }

        public async Task<BaseResult<Salary>> Handle(CalculateSalaryTaxablesCommand request, CancellationToken cancellationToken)
        {
            var salary = _calculator.Calculate(
                request.GrossSalary,
                request.PayNationalInsurance,
                request.HasStudentLoan,
                request.PensionPercentage,
                request.StudentPlanType
            );

            return BaseResult<Salary>.SuccessResult(salary);
        }

        public class CalculateSalaryTaxablesCommandValidator : AbstractValidator<CalculateSalaryTaxablesCommand>
        {
            public CalculateSalaryTaxablesCommandValidator()
            {
                RuleFor(x => x.GrossSalary)
                    .GreaterThan(0).WithMessage("Gross salary must be greater than 0");

                RuleFor(x => x.PensionPercentage)
                    .InclusiveBetween(0, 100)
                    .WithMessage("Pension percentage must be between 0 and 100");

                RuleFor(x => x.StudentPlanType)
                    .NotNull()
                    .When(x => x.HasStudentLoan)
                    .WithMessage("Student plan type must be set when HasStudentLoan is true");
            }
        }
    }
}

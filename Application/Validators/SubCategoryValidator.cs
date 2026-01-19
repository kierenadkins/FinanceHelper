using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FluentValidation;

namespace FinanceHelper.Application.Validators;

public class SubCategoryValidator : AbstractValidator<SubCategory>
{
    public SubCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.SubCategoryType)
            .IsInEnum().WithMessage("CategoryType must be a valid value.");
        RuleFor(x => x.PayFrequency)
            .IsInEnum().WithMessage("PayFrequency must be a valid value.");

        RuleFor(x => x.YearlyCost)
            .GreaterThanOrEqualTo(0).WithMessage("YearlyCost cannot be negative.");

        RuleFor(x => x.MonthlyCost)
            .GreaterThanOrEqualTo(0).WithMessage("MonthlyCost cannot be negative.");

        RuleFor(x => x.WeeklyCost)
            .GreaterThanOrEqualTo(0).WithMessage("WeeklyCost cannot be negative.");
    }
}
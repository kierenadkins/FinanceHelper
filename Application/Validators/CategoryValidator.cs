using Application.Domain.Finance.ExpenseTracking;
using FluentValidation;

namespace Application.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("User must be assigned");
        RuleFor(x => x.Type).NotNull().WithMessage("Category type is required.");
    }
}
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class SaveCategoryCommand : IRequest<BaseResult>
{
    public CategoryType CategoryType;
}

public class SaveCategoryCommandHandler(IUserAccountService userAccountService, ICategoryService categoryService)
    : IRequestHandler<SaveCategoryCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult();
        var userId = userAccountService.GetCurrent();

        var category = new FinanceHelper.Domain.Objects.Finance.ExpenseTracking.Category { UserId = userId, Type = request.CategoryType };

        var categories = await categoryService.GetAllCategoriesWithUserIdCached(userId);

        if (categories.Any(x => x.Type == category.Type))
        {
            result.AddError("This category already exists for your account.");
            return result;
        }

        await categoryService.AddAsync(category, userId);
        return result;
    }

    public class SaveCategoryCommandValidator : AbstractValidator<SaveCategoryCommand>
    {
        public SaveCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryType).NotNull().WithMessage("Category type is required.");
        }
    }
}
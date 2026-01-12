using Application.Domain.Base;
using Application.Domain.Finance.ExpenseTracking;
using Application.Services.Finance.ExpenseTracking;
using Application.Services.User;
using Application.Validators;
using MediatR;

namespace Application.Usecases.Category.Command;

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

        var category = new Domain.Finance.ExpenseTracking.Category { UserId = userId, Type = request.CategoryType };

        var catValidator = new CategoryValidator();
        var catValidationResult = await catValidator.ValidateAsync(category, cancellationToken);

        if (!catValidationResult.IsValid)
        {
            result.AddErrors(catValidationResult.Errors.Select(e => e.ErrorMessage));
            return result;
        }

        //if (request.Category.SubCategories.Any())
        //{
        //    var subValidator = new SubCategoryValidator();

        //    foreach (var subCategory in request.Category.SubCategories)
        //    {
        //        var validationResult = await subValidator.ValidateAsync(subCategory, cancellationToken);

        //        if (validationResult.IsValid) continue;

        //        result.AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
        //        return result;
        //    }
        //}

        var categories = await categoryService.GetAllCategoriesWithUserIdCached(userId);

        if (categories.Count > 0 & categories.Any(x => x.Type == category.Type))
        {
            result.AddError("This category already exists for your account");
            return result;
        }

        await categoryService.AddAsync(category, userId);
        return result;
    }
}
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Validators;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class AddSubCategoryCommand : IRequest<BaseResult>
{
    public SubCategory SubCategory;
}

public class AddSubCategoryCommandHandler(
    ISubCategoryService subCategoryService,
    IUserAccountService userAccountService)
    : IRequestHandler<AddSubCategoryCommand, BaseResult>
{
    public async Task<BaseResult> Handle(AddSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult();

        var validator = new SubCategoryValidator();
        var validationResult = await validator.ValidateAsync(request.SubCategory, cancellationToken);

        if (!validationResult.IsValid)
        {
            result.AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
            return result;
        }

        var userId = userAccountService.GetCurrent();
        await subCategoryService.AddAsync(request.SubCategory, userId);
        return result;
    }
}
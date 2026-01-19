using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Validators;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class UpdateSubCategoryCommand : IRequest<BaseResult>
{
    public SubCategory SubCategory;
}

public class UpdateSubCategoryCommandHandler(
    ISubCategoryService subCategoryService,
    IUserAccountService userAccountService)
    : IRequestHandler<UpdateSubCategoryCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
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
        await subCategoryService.UpdateAsync(request.SubCategory, userId);
        return result;
    }
}
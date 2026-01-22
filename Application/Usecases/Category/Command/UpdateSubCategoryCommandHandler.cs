using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class UpdateSubCategoryCommand : IRequest<BaseResult>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SubCategoryType SubCategoryType { get; set; }
    public PaymentFrequency PayFrequency { get; set; }
    public int CategoryId { get; set; }
    public decimal YearlyCost { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal WeeklyCost { get; set; }
}

public class UpdateSubCategoryCommandHandler(
    ISubCategoryService subCategoryService,
    IUserAccountService userAccountService)
    : IRequestHandler<UpdateSubCategoryCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult();
        var userId = userAccountService.GetCurrent();

        var subCategory = new SubCategory
        {
            Id = request.Id,
            Name = request.Name,
            SubCategoryType = request.SubCategoryType,
            PayFrequency = request.PayFrequency,
            CategoryId = request.CategoryId,
            YearlyCost = request.YearlyCost,
            MonthlyCost = request.MonthlyCost,
            WeeklyCost = request.WeeklyCost,
        };

        await subCategoryService.UpdateAsync(subCategory, userId);
        return result;
    }
}

public class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommand>
{
    public UpdateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().NotEqual(0).WithMessage("Id is required.");
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
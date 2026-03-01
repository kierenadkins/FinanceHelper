using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Domain.Objects.Accounts;
using FinanceHelper.Domain.Objects.Base;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class UpdateSavingAccount : IRequest<BaseResult>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public AccountType AccountType { get; set; }
    public decimal InterestRate { get; set; }
    public InterestType InterestType { get; set; }
}

public class UpdateSavingAccountHandler(ISavingService _savingService)
    : IRequestHandler<UpdateSavingAccount, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateSavingAccount request, CancellationToken cancellationToken)
    {
        try
        {
            var savingsAccount = await _savingService.GetByIdWithTransactionsAsync(request.Id);

            if (savingsAccount == null)
                return new BaseResult("Saving account not found.");

            savingsAccount.UpdateSavingAccount(request.Name, request.Provider, request.AccountType, request.InterestRate, request.InterestType);

            // Pass user ID to invalidate cache
            await _savingService.UpdateAsync(savingsAccount, savingsAccount.UserId);
            return new BaseResult();
        }
        catch (Exception ex)
        {
            return new BaseResult($"Failed to update account: {ex.Message}");
        }
    }

    public class SaveSavingsAccountValidator : AbstractValidator<SaveSavingsAccount>
    {
        public SaveSavingsAccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Provider)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AccountType)
                .IsInEnum();

            RuleFor(x => x.InterestRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.InterestType)
                .IsInEnum();
        }
    }
}
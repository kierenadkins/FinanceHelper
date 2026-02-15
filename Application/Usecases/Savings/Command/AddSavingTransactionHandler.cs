using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class AddSavingTransaction : IRequest<BaseResult>
{
    public int id { get; set; }
    public decimal Amount { get; set; }

}

public class AddSavingTransactionHandler(ISavingService _savingService, IUserAccountService _UserAccount)
    : IRequestHandler<AddSavingTransaction, BaseResult>
{
    public async Task<BaseResult> Handle(AddSavingTransaction request, CancellationToken cancellationToken)
    {

        var savingAccount = await _savingService.GetByIdAsync(request.id);

        if (savingAccount == null)
        {
            return new BaseResult("No such saving account exists");
        }

        savingAccount.AddTransaction(request.Amount);

        await _savingService.UpdateAsync(savingAccount);

        return new BaseResult();
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

            RuleFor(x => x.InitalBalance)
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
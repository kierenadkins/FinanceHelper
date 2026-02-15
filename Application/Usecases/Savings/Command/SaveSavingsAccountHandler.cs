using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Accounts;
using FinanceHelper.Domain.Objects.Base;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Category.Command;

public class SaveSavingsAccount : IRequest<BaseResult>
{
    public string Name { get; set; }
    public string Provider { get; set; }
    public decimal InitalBalance { get; set; }
    public AccountType AccountType { get; set; }
    public decimal InterestRate { get; set; }
    public InterestType InterestType { get; set; }
}

public class SaveSavingsAccountHandler(ISavingService _savingService, IUserAccountService _UserAccount)
    : IRequestHandler<SaveSavingsAccount, BaseResult>
{
    public async Task<BaseResult> Handle(SaveSavingsAccount request, CancellationToken cancellationToken)
    {
        var user = _UserAccount.GetCurrent();

        if (user == 0)
        {
            return new BaseResult("User does not exist");
        }

        var savingAccount = new SavingAccount(user, request.Name, request.Provider, request.AccountType, request.InterestRate, request.InitalBalance, request.InterestType);

        await _savingService.AddAsync(savingAccount);

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
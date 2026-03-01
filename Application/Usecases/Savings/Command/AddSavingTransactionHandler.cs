using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using MediatR;

namespace FinanceHelper.Application.Usecases.Savings.Command;

public class AddSavingTransaction : IRequest<BaseResult>
{
    public int id { get; set; }
    public decimal Amount { get; set; }

}

public class AddSavingTransactionHandler(ISavingService savingService, IUserAccountService _UserAccount) : IRequestHandler<AddSavingTransaction, BaseResult>
{
    public async Task<BaseResult> Handle(AddSavingTransaction request, CancellationToken cancellationToken)
    {

        var savingAccount = await savingService.GetByIdAsync(request.id);

        if (savingAccount == null)
        {
            return new BaseResult("No such saving account exists");
        }

        savingAccount.AddTransaction(request.Amount);

        await savingService.UpdateAsync(savingAccount);

        return new BaseResult();
    }
}
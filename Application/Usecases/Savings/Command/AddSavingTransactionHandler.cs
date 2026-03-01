using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Base;
using MediatR;

namespace FinanceHelper.Application.Usecases.Savings.Command;

public class AddSavingTransaction : IRequest<BaseResult>
{
    public int id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = "Transaction";
}

public class AddSavingTransactionHandler(ISavingService savingService, IUserAccountService userAccountService) : IRequestHandler<AddSavingTransaction, BaseResult>
{
    public async Task<BaseResult> Handle(AddSavingTransaction request, CancellationToken cancellationToken)
    {
        var savingAccount = await savingService.GetByIdWithTransactionsAsync(request.id);

        if (savingAccount == null)
        {
            return new BaseResult("No such saving account exists");
        }

        try
        {
            savingAccount.AddTransaction(request.Amount, request.Description);
            
            // Invalidate cache by passing the user ID
            await savingService.UpdateAsync(savingAccount, savingAccount.UserId);
            
            return new BaseResult();
        }
        catch (InvalidOperationException ex)
        {
            return new BaseResult(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return new BaseResult(ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResult($"Failed to add transaction: {ex.Message}");
        }
    }
}
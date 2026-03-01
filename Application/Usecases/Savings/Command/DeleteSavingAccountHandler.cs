using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Domain.Objects.Base;
using MediatR;

namespace FinanceHelper.Application.Usecases.Savings.Command;

public class DeleteSavingAccount : IRequest<BaseResult>
{
    public int Id { get; set; }
}

public class DeleteSavingAccountHandler(ISavingService savingService) : IRequestHandler<DeleteSavingAccount, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteSavingAccount request, CancellationToken cancellationToken)
    {
        try
        {
            var savingAccount = await savingService.GetByIdAsync(request.Id);

            if (savingAccount == null)
                return new BaseResult("Saving account not found.");

            // Pass the UserId to invalidate cache
            await savingService.DeleteAsync(savingAccount, savingAccount.UserId);

            return new BaseResult();
        }
        catch (Exception ex)
        {
            return new BaseResult($"Failed to delete account: {ex.Message}");
        }
    }
}

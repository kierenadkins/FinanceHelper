using FinanceHelper.Application.Domain.Dto;
using FinanceHelper.Application.Services.Finance;
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Accounts;
using FinanceHelper.Domain.Objects.Base;
using MediatR;

namespace FinanceHelper.Application.Usecases.Finance
{
    public class GetSavingAccount : IRequest<SavingAccount?>
    {
        public int Id { get; set; }
    }

    public class GetSavingAccountQuery(ISavingService savingService) : IRequestHandler<GetSavingAccount, SavingAccount?>
    {
        public async Task<SavingAccount?> Handle(GetSavingAccount request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return null;

            return await savingService.GetByIdWithTransactionsAsync(request.Id);
        }
    }
}
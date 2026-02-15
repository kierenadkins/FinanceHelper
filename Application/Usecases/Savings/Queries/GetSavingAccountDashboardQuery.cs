using FinanceHelper.Application.Domain.Dto;
using FinanceHelper.Application.Services.Finance;
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.Savings;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Accounts;
using MediatR;

namespace FinanceHelper.Application.Usecases.Finance
{
    public class GetSavingAccountDashboard : IRequest<List<SavingAccount>>
    {
    }

    public class GetSavingAccountDashboardQuery(
        IUserAccountService userAccountService, ISavingService savingService)
        : IRequestHandler<GetSavingAccountDashboard, List<SavingAccount>>
    {
        public Task<List<SavingAccount>> Handle(GetSavingAccountDashboard request, CancellationToken cancellationToken)
        {
            var userId = userAccountService.GetCurrent();

            if (userId == 0)
            {
                return null;
            }

            var savings = savingService.GetAllSavingsWithTransactionsWithUserIdCached(userId);

            return savings;
        }
    }
}
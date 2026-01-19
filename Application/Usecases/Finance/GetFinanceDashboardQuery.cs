using FinanceHelper.Application.Domain.Dto;
using FinanceHelper.Application.Services.Finance;
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using MediatR;

namespace FinanceHelper.Application.Usecases.Finance
{
    public class GetFinanceDashboardQuery : IRequest<FinanceDto?>
    {
    }

    public class GetFinanceDashboardQueryHandler(
        ISalaryService salaryService,
        IUserAccountService userAccountService,
        ICategoryService categoryService)
        : IRequestHandler<GetFinanceDashboardQuery, FinanceDto?>
    {
        public async Task<FinanceDto?> Handle(GetFinanceDashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = userAccountService.GetCurrent();

            if (userId == 0)
            {
                return null;
            }

            var salary = await salaryService.GetAllByUserIdCacheAsync(userId);

            if (salary == null)
            {
                return null;
            }

            var catsWithSubCats = await categoryService.GetAllCategoriesWithSubCategoriesWithUserIdCached(userId);

            var finance = new FinanceDto(salary, catsWithSubCats);

            return finance;
        }
    }
}
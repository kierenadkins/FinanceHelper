using FinanceHelper.Domain.Objects.Finance;

namespace FinanceHelper.Application.Services.Finance
{
    public interface ISalaryService : IGenericCrudService<Salary>
    {
        Task<Salary?> GetAllByUserIdCacheAsync(int userId);
    }
}
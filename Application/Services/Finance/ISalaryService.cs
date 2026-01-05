using Application.Domain.Finance;

namespace Application.Services.Finance
{
    public interface ISalaryService : IGenericCrudService<Salary>
    {
        Task<Salary?> GetAllByUserIdCacheAsync(int userId);
    }
}
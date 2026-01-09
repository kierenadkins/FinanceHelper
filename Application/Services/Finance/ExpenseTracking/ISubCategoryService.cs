using Application.Domain.Finance.ExpenseTracking;

namespace Application.Services.Finance.ExpenseTracking;

public interface ISubCategoryService
{
    Task<SubCategory?> GetByIdAsync(int id);
    Task<SubCategory> AddAsync(SubCategory entity, params object[] cacheArgs);
    Task AddRangeAsync(List<SubCategory> entities, params object[] cacheArgs);
    Task UpdateAsync(SubCategory entity, params object[] cacheArgs);
    Task UpdateAsync(IEnumerable<SubCategory> entities, params object[] cacheArgs);
    Task DeleteAsync(int id, params object[] cacheArgs);
    Task DeleteAsync(SubCategory entity, params object[] cacheArgs);
}
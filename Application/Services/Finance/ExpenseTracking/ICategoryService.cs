using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

namespace FinanceHelper.Application.Services.Finance.ExpenseTracking;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoriesWithUserIdCached(int userId);
    Task<List<Category>> GetAllCategoriesWithSubCategoriesWithUserIdCached(int userId);
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category entity, params object[] cacheArgs);
    Task AddRangeAsync(List<Category> entities, params object[] cacheArgs);
    Task UpdateAsync(Category entity, params object[] cacheArgs);
    Task UpdateAsync(IEnumerable<Category> entities, params object[] cacheArgs);
    Task DeleteAsync(int id, params object[] cacheArgs);
    Task DeleteAsync(Category entity, params object[] cacheArgs);
}
using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Services.User
{
    public class UserAccountService(IRepository<UserAccount> repository,
    ISessionManagerService sessionManager,
    ICacheManagerService cacheManager,
    IFinanceHelperDbContext ctx,
    IEntityCacheKey<UserAccount> cacheKeys
) : GenericCrudService<UserAccount>(repository, cacheManager, ctx, cacheKeys), IUserAccountService
    {
        private const int CacheDurationSeconds = 3600;

        public async Task<UserAccount?> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var cacheKey = CacheKeys.UserByEmail(email);

            if (cacheManager.IsSet(cacheKey))
                return cacheManager.Get<UserAccount>(cacheKey);

            var user = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.EmailAddress == email);

            if (user != null)
                cacheManager.Set(user, cacheKey, CacheDurationSeconds);

            return user;
        }

        public int GetCurrent()
        {
            var userSession = sessionManager.Get<int>("CurrentUser");
            return userSession;
        }
        private void RemoveCache(string email)
        {
            var cacheKey = CacheKeys.UserByEmail(email);
            cacheManager.Remove(cacheKey);
            cacheManager.Remove("AllUserAccounts");
        }
    }
}

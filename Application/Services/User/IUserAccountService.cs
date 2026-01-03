using Application.Domain.Users;

namespace Application.Services.User
{
    public interface IUserAccountService
    {
        int GetCurrent();
        Task<UserAccount?> GetUserByEmail(string email);


        //generic
        Task<UserAccount?> GetByIdAsync(int id);
        Task<UserAccount> AddAsync(UserAccount account, params object[] cacheArgs);
        Task UpdateAsync(UserAccount account, params object[] cacheArgs);
        Task UpdateAsync(IEnumerable<UserAccount> accounts, params object[] cacheArgs);
        Task DeleteAsync(UserAccount account, params object[] cacheArgs);
        Task DeleteAsync(int id, params object[] cacheArgs);
    }
}
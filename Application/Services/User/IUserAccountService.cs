using Application.Domain.Users;

namespace Application.Services.User
{
    public interface IUserAccountService
    {
        Task Add(UserAccount userAccount);
        Task Delete(int id);
        Task<List<UserAccount>> GetAllCached();
        int GetCurrent();
        Task<UserAccount?> GetUserByEmail(string email);
        Task Update(UserAccount userAccount);
    }
}
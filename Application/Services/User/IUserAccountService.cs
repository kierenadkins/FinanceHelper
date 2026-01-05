using Application.Domain.Users;

namespace Application.Services.User
{
    public interface IUserAccountService : IGenericCrudService<UserAccount>
    {
        int GetCurrent();
        Task<UserAccount?> GetUserByEmail(string email);
    }
}
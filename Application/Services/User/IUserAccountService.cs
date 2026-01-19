using FinanceHelper.Domain.Objects.Users;

namespace FinanceHelper.Application.Services.User
{
    public interface IUserAccountService : IGenericCrudService<UserAccount>
    {
        int GetCurrent();
        Task<UserAccount?> GetUserByEmail(string email);
    }
}
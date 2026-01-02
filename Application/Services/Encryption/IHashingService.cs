
namespace Application.Services.Encryption
{
    public interface IHashingService
    {
        Task<string> Hash(string originalString, CancellationToken cancellationToken = default);
        string HashPassword(string password);
        bool VerifyHash(string password, string hashAndSalt);
    }
}
namespace Application.Services.Encryption
{
    public interface IEncryptionService
    {
        string Decrypt(string encryptedText);
        string Encrypt(string text);
    }
}
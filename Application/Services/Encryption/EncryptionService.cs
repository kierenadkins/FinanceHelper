using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private string EncryptString(string text, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            var plainTextBytes = Encoding.UTF8.GetBytes(text);

            var password =
                new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            var keyBytes = password.GetBytes(keySize / 8);

            using (var symmetricKey = Aes.Create())
            {
                symmetricKey.Mode = CipherMode.CBC;

                var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                        cryptoStream.FlushFinalBlock();

                        var cipherTextBytes = memoryStream.ToArray();

                        memoryStream.Close();
                        cryptoStream.Close();

                        var cipherText = Convert.ToBase64String(cipherTextBytes);
                        return cipherText;
                    }
                }
            }
        }

        private string DecryptString(string encryptedText, string passPhrase, string saltValue,
            string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            var cipherTextBytes = Convert.FromBase64String(encryptedText);

            var password =
                new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            var keyBytes = password.GetBytes(keySize / 8);

            using (var symmetricKey = Aes.Create())
            {
                symmetricKey.Mode = CipherMode.CBC;

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {

                        var plainTextBytes = new byte[cipherTextBytes.Length];

                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        memoryStream.Close();
                        cryptoStream.Close();

                        var plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                        return plainText;
                    }
                    ;
                }
            }
        }

        public string Encrypt(string text)
        {
            var passPhrase = "hyEfgteDfh";
            var saltValue = "egyu34gh4t";
            var hashAlgorithm = "SHA1";
            var initVector = "@KE2cIe4e9W6K792"; // must be 16 bytes

            var passwordIterations = 2;
            var keySize = 256;

            return EncryptString(text, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        public string Decrypt(string encryptedText)
        {
            var passPhrase = "hyEfgteDfh";
            var saltValue = "egyu34gh4t";
            var hashAlgorithm = "SHA1";
            var initVector = "@KE2cIe4e9W6K792"; // must be 16 bytes

            var passwordIterations = 2;
            var keySize = 256;

            encryptedText = encryptedText.Replace('-', '+')
                .Replace(" ", "+")
                .Replace('_', '/')
                .PadRight(4 * ((encryptedText.Length + 3) / 4), '=');

            return DecryptString(encryptedText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);

        }
    }
}

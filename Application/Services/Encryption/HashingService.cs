using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FinanceHelper.Application.Services.Encryption
{
    public class HashingService : IHashingService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool VerifyHash(string password, string hashAndSalt)
        {
            string[] hashComponents = hashAndSalt.Split('-');
            byte[] hash = Convert.FromHexString(hashComponents[0]);
            byte[] salt = Convert.FromHexString(hashComponents[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        public async Task<string> Hash(string originalString, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(originalString))
                return string.Empty;

            byte[] result;

            var hashData = Encoding.ASCII.GetBytes(originalString);

            using (var sha256 = SHA256.Create())
            {
                result = sha256.ComputeHash(hashData);
            }

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}

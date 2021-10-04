namespace ChatApp.Api.HttpIn.Authentication
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using Domain.Services;

    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password,
                SaltSize,
                10000,
                HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);
            return $"{salt}.{key}";
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);
            using var algorithm = new Rfc2898DeriveBytes(password,
                salt,
                10000,
                HashAlgorithmName.SHA512);
            var keyToCheck = algorithm.GetBytes(KeySize);
            return keyToCheck.SequenceEqual(key);
        }
    }
}
using PWManager.Domain.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PWManager.Application.Services {
    public class CryptService : ICryptService {
        private byte[] _iv { get; set; }

        public CryptService(byte[] initializationVector) {
            this._iv = initializationVector;
        }

        public string Decrypt(string input, string key) {
            var inputBytes = Convert.FromBase64String(input);

            using var aes = CreateAesWith(key);
            var plain = aes.DecryptCfb(inputBytes, _iv);

            return Encoding.ASCII.GetString(plain);
        }

        public string Encrypt(string input, string key) {
            var inputBytes = Encoding.ASCII.GetBytes(input);

            using var aes = CreateAesWith(key);
            var cipher = aes.EncryptCfb(inputBytes, _iv);

            return Convert.ToBase64String(cipher);
        }

        public string HashForLogin(string input, string salt) {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var saltBytes = Encoding.ASCII.GetBytes(salt);

            var saltedInput = inputBytes.Concat(saltBytes).ToArray();
            var hashed = SHA512.HashData(saltedInput);

            return Convert.ToBase64String(hashed);
        }

        public string DeriveKeyFrom(string input, string salt) {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var saltBytes = Encoding.ASCII.GetBytes(salt);

            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(inputBytes, saltBytes, 210000, HashAlgorithmName.SHA512, 32);
            
            return Encoding.ASCII.GetString(hashBytes);
        }

        private static Aes CreateAesWith(string key) {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var aes = Aes.Create();
            aes.Key = keyBytes;

            return aes;
        }
    }
}


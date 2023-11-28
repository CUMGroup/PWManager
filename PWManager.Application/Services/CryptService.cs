using PWManager.Domain.Services.Interfaces;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;

namespace PWManager.Application.Services {
    public class CryptService : ICryptService {

        public CryptService() {
        }

        public string Decrypt(string input, string key) {
            var inputBytes = Convert.FromBase64String(input);

            using var aes = CreateAesWith(key);
            var iv = inputBytes[0..16];
            var plainBytes = aes.DecryptCfb(inputBytes[16..], iv);

            return Encoding.ASCII.GetString(plainBytes);
        }

        public string Encrypt(string input, string key) {
            var inputBytes = Encoding.ASCII.GetBytes(input);

            using var aes = CreateAesWith(key);
            aes.GenerateIV();
            var iv = aes.IV;

            var cipherBytes = aes.EncryptCfb(inputBytes, iv);
            var cipherWithIvHeading = iv.Concat(cipherBytes).ToArray();

            return Convert.ToBase64String(cipherWithIvHeading);
        }

        public string HashForLogin(string input, string salt) {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var saltBytes = Encoding.ASCII.GetBytes(salt);

            var saltedInput = inputBytes.Concat(saltBytes).ToArray();
            var hashBytes = SHA512.HashData(saltedInput);

            return Convert.ToBase64String(hashBytes);
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


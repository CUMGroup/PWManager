using PWManager.Domain.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PWManager.Application.Services {
    public class CryptService : ICryptService {
        private byte[] InitializationVector { get; set; }

        public CryptService(byte[] initializationVector) {
            this.InitializationVector = initializationVector;
        }

        public string Decrypt(byte[] input, byte[] key) {
            var aes = AesCng.Create();
            var plain = aes.DecryptCfb(input, this.InitializationVector);
            return plain.ToString();
        }

        public string Encrypt(byte[] input, byte[] key) {
            var aes = AesCng.Create();
            var cipher = aes.EncryptCfb(input, this.InitializationVector);
            return cipher.ToString();
        }

        public string HashForLogin(byte[] input) { 
            return SHA512.HashData(input).ToString();
        }

        public string HashKey(byte[] input, byte[] salt) { 
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(input, salt, 210000, HashAlgorithmName.SHA512, 256);
            return hashBytes.ToString();
        }
    }
}


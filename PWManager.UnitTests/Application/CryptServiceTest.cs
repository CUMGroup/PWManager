using PWManager.Application.Services;
using System.Security.Cryptography;
using System.Text;

namespace PWManager.UnitTests.Application {
    public class CryptServiceTest {

        [Fact]
        public void CryptService_Should_HashTheSamePassordIdentical() {
            var testPassword = Encoding.ASCII.GetBytes("password");

            var aes = Aes.Create();
            aes.GenerateIV();
            var sut = new CryptService(aes.IV);

            var hash1 = sut.HashForLogin(testPassword);
            var hash2 = sut.HashForLogin(testPassword);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void CryptService_Should_EncryptAndDecryptToGetTheMessageBack() {
            var testPassword = Encoding.ASCII.GetBytes("password");
            var testSalt = Encoding.ASCII.GetBytes("salt");
            var testPlain = Encoding.ASCII.GetBytes("Secret Message");

            var aes = Aes.Create();
            aes.GenerateIV();
            var sut = new CryptService(aes.IV);

            var key = sut.HashKey(testPassword, testSalt);
            var cipher = sut.Encrypt(testPlain, Encoding.ASCII.GetBytes(key));
            var plain = sut.Decrypt(Encoding.ASCII.GetBytes(cipher), Encoding.ASCII.GetBytes(key));

            Assert.Equal(testPlain.ToString(), plain);
        }

    }
}

using PWManager.Application.Services;
using System.Security.Cryptography;
using System.Text;

namespace PWManager.UnitTests.Application {
    public class CryptServiceTest {

        [Fact]
        public void CryptService_Should_HashTheSamePassordIdentical() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService();

            var hash1 = sut.HashForLogin(testPassword, testSalt);
            var hash2 = sut.HashForLogin(testPassword, testSalt);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void DeriveKeyFrom_Should_ReturnTheSame_ForTheSameInput() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService();

            var key1 = sut.DeriveKeyFrom(testPassword, testSalt);
            var key2 = sut.DeriveKeyFrom(testPassword, testSalt);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void HashForLogin_ShouldNot_ReturnTheSameAs_DeriveKeyFrom() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService();

            var hash1 = sut.HashForLogin(testPassword, testSalt);
            var key = sut.DeriveKeyFrom(testPassword, testSalt);

            Assert.NotEqual(hash1, key);
        }

        [Fact]
        public void CryptService_Should_EncryptAndDecryptToGetTheMessageBack() {
            var testPassword = "password";
            var testSalt = "salt";
            var testPlain = "Secret Message";

            var sut = new CryptService();

            var key = sut.DeriveKeyFrom(testPassword, testSalt);
            var cipher = sut.Encrypt(testPlain, key);
            var plain = sut.Decrypt(cipher, key);

            Assert.Equal(testPlain, plain);
        }

    }
}

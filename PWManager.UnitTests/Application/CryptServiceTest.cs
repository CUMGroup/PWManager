using PWManager.Application.Services;
using System.Security.Cryptography;
using System.Text;
using NSubstitute;
using PWManager.Application.Context;

namespace PWManager.UnitTests.Application {
    public class CryptServiceTest {

        [Fact]
        public void CryptService_Should_HashTheSamePassordIdentical() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService(null);

            var hash1 = sut.Hash(testPassword, testSalt);
            var hash2 = sut.Hash(testPassword, testSalt);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void DeriveKeyFrom_Should_ReturnTheSame_ForTheSameInput() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService(null);

            var key1 = sut.DeriveKeyFrom(testPassword, testSalt);
            var key2 = sut.DeriveKeyFrom(testPassword, testSalt);

            Assert.Equal(key1, key2);
        }

        [Fact]
        public void Hash_ShouldNot_ReturnTheSameAs_DeriveKeyFrom() {
            var testPassword = "password";
            var testSalt = "salt";

            var sut = new CryptService(null);

            var hash1 = sut.Hash(testPassword, testSalt);
            var key = sut.DeriveKeyFrom(testPassword, testSalt);

            Assert.NotEqual(hash1, key);
        }

        [Fact]
        public void CryptService_Should_EncryptAndDecryptToGetTheMessageBack() {
            var testPassword = "password";
            var testSalt = "salt";
            var testPlain = "Secret Message";
            
            var env = Substitute.For<ICryptEnvironment>();
            env.EncryptionKey.Returns("f??RH!\u0016???,?@?/V??V7R???n??\u0014? Qx");
            var sut = new CryptService(env);

            var key = sut.DeriveKeyFrom(testPassword, testSalt);
            var cipher = sut.Encrypt(testPlain);
            var plain = sut.Decrypt(cipher);

            Assert.Equal(testPlain, plain);
        }

    }
}

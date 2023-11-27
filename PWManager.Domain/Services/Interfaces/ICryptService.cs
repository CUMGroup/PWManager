namespace PWManager.Domain.Services.Interfaces {
    public interface ICryptService {
        public string Encrypt(string input, string key);
        public string Decrypt(string input, string key);
        public string HashForLogin(string input, string salt);
        public string DeriveKeyFrom(string input, string salt);
    }
}
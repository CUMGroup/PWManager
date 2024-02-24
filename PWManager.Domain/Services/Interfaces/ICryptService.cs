namespace PWManager.Domain.Services.Interfaces {
    public interface ICryptService {

        public void Encrypt(ISecureProperties input, string key);
        public void Decrypt(ISecureProperties input, string key);

        public string Encrypt(string input);
        public string Decrypt(string input);

        public string Hash(string input, string salt);
        public string DeriveKeyFrom(string input, string salt);

        public string GenerateSalt();
    }
}
namespace PWManager.Domain.Services.Interfaces {
    public interface ICryptService {
        public string Encrypt(byte[] input, byte[] key);
        public string Decrypt(byte[] input, byte[] key);
        public string HashForLogin(byte[] input);
        public string HashKey(byte[] input, byte[] salt);
    }
}
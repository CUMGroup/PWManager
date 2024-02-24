namespace PWManager.Domain.Services.Interfaces {
    public interface ISecureProperties {
        public Dictionary<string, string> GetProperties();
        public void SecureProperties(Dictionary<string, string> securedData);
    }
}

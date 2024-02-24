namespace PWManager.Domain.Services.Interfaces {
    public interface ISecureProperties {
        public List<(Func<string>, Action<string>)> SecurableProperties();
    }
}

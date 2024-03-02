namespace PWManager.Application.Context; 

public interface ICryptEnvironment {
    public string? EncryptionKey { get; set; }
}
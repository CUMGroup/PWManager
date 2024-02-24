namespace PWManager.Application.Context; 

public interface IApplicationEnvironment {

    public bool IsDevelopmentMode { get; init; }
    
    public bool RunningSession { get; set; }
    
    public string? UserName { get; init; }
    public string? UserId { get; init; }
    
    public string? CurrentGroup { get; set; }
    
    public string? EncryptionKey { get; init; }
}
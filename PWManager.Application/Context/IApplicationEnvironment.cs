using PWManager.Domain.Entities;

namespace PWManager.Application.Context; 

public interface IApplicationEnvironment {

    public bool IsDevelopmentMode { get; init; }
    
    public bool RunningSession { get; set; }

    public User? CurrentUser { get; set; }
    
    public string? CurrentGroup { get; set; }
    
    public string? EncryptionKey { get; init; }
}
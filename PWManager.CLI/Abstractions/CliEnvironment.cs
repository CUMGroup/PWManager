using PWManager.Application.Context;

namespace PWManager.CLI.Abstractions; 

public class CliEnvironment : IApplicationEnvironment {

    public bool IsDevelopmentMode { get; init; } = true;

    public bool RunningSession { get; set; } = false;
    public string Prompt => $"{UserName} ({CurrentGroup}) $";

    public string? UserName { get; init; }
    public string? UserId { get; init; }
    
    public string? CurrentGroup { get; set; }
    
    public string? EncryptionKey { get; init; }
}
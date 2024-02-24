using PWManager.Application.Context;
using PWManager.Domain.Entities;

namespace PWManager.CLI.Abstractions; 

public class CliEnvironment : IApplicationEnvironment {

    public bool IsDevelopmentMode { get; init; } = true;

    public bool RunningSession { get; set; } = false;
    public string Prompt => $"{CurrentUser.UserName} ({CurrentGroup}) $";

    public User? CurrentUser { get; set; }
    
    public string? CurrentGroup { get; set; }
    
    public string? EncryptionKey { get; init; }
}
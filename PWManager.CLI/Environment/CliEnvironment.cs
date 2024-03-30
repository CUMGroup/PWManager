using PWManager.Application.Context;
using PWManager.Domain.Entities;

namespace PWManager.CLI.Environment; 

public class CliEnvironment : ICliEnvironment, IDebugEnvironment, IUserEnvironment, ICryptEnvironment, ICancelEnvironment {
    
    public bool IsDevelopmentMode { get; init; } = true;

    public bool RunningSession { get; set; } = false;

    public void WritePrompt() {
        var defaultColor = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(CurrentUser?.UserName ?? "User");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write($" ({CurrentGroup?.Identifier ?? "Group"}) ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("$ ");
        Console.ForegroundColor = defaultColor;
        
    }

    public User? CurrentUser { get; set; }
    
    public Group? CurrentGroup { get; set; }
    
    public Settings? UserSettings { get; set; }
    
    public string? EncryptionKey { get; set; }
    
    public bool CancelableState { get; set; }
}
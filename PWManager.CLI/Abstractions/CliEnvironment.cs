namespace PWManager.CLI.Abstractions; 

public class CliEnvironment {

    public bool IsDevelopmentMode { get; } = true;

    public bool RunningSession { get; set; } = false;
    public string Prompt => $"{UserName} ({CurrentGroup}) $";

    public string UserName { get; set; }
    public string UserId { get; set; }
    
    public string CurrentGroup { get; set; }
}
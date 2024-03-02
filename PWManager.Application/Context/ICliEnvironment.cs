namespace PWManager.Application.Context; 

public interface ICliEnvironment {
    public bool RunningSession { get; set; }
    public void WritePrompt();
}
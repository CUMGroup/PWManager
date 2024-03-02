namespace PWManager.Application.Context; 

public interface IDebugEnvironment {
    public bool IsDevelopmentMode { get; init; }
}
namespace PWManager.Application.Context; 

public interface ICancelEnvironment {
    bool CancelableState { get; set; }
}
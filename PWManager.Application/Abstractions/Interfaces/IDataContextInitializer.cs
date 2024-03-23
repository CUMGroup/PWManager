namespace PWManager.Application.Abstractions.Interfaces; 

public interface IDataContextInitializer {
    
    bool DatabaseExists(string path);
    void InitDataContext(string path);
}
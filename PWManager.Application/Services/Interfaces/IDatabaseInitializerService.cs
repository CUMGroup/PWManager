namespace PWManager.Application.Services.Interfaces; 

public interface IDatabaseInitializerService {

    void InitDatabase(string path, string username, string password);

    void CheckIfDataBaseExistsOnPath(string path);
}
using PWManager.Data.Persistance;

namespace PWManager.Data; 

public static class DataContext {

    private static ApplicationDbContext? _dbContext;
    private const string DatabaseFileName = "pwdb.scuml";
    
    public static bool DatabaseExists(string path) {
        return File.Exists(Path.Combine(path, DatabaseFileName));
    }
    
    public static void InitDataContext(string path) {
        if (_dbContext is not null) {
            return;
        }
        _dbContext = new ApplicationDbContext(Path.Combine(path, DatabaseFileName));

        _dbContext.Database.EnsureCreated();
    }

    internal static ApplicationDbContext GetDbContext() {
        if (_dbContext is null) {
            throw new ApplicationException("Data Context was not initialized!");
        }
        return _dbContext;
    }
}
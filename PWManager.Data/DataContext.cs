using PWManager.Data.Persistance;

namespace PWManager.Data; 

public static class DataContext {

    private static ApplicationDbContext? _dbContext;

    public static void InitDataContext(string path) {
        if (_dbContext is not null) {
            return;
        }
        _dbContext = new ApplicationDbContext(Path.Combine(path, "pwdb.scuml"));

        _dbContext.Database.EnsureCreated();
    }

    internal static ApplicationDbContext GetDbContext() {
        if (_dbContext is null) {
            throw new ApplicationException("Data Context was not initialized!");
        }
        return _dbContext;
    }
}
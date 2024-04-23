using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Exceptions;
using PWManager.Data.Abstraction.Interfaces;
using PWManager.Data.Persistance;

namespace PWManager.Data; 

public class DataContext : IDataContextInitializer, IHaveDataContext, IDeleteDataContext {

    private ApplicationDbContext? _dbContext;
    private const string DatabaseFileName = "pwdb.scuml";
    
    public bool DatabaseExists(string path) {
        return File.Exists(Path.Combine(path, DatabaseFileName));
    }
    
    public void InitDataContext(string path) {
        if (_dbContext is not null) {
            return;
        }
        _dbContext = new ApplicationDbContext(Path.Combine(path, DatabaseFileName));
        
        _dbContext.Database.EnsureCreated();
    }

    private void CloseDatabase() {
        if (_dbContext is null) {
            return;
        }
        _dbContext.Dispose();
        _dbContext = null;
    }

    public void DeleteDataContext() {
        if (_dbContext is null) {
            return;
        }

        _dbContext.Database.EnsureDeleted();
        
        CloseDatabase();
    }

    ApplicationDbContext IHaveDataContext.GetDbContext() {
        if (_dbContext is null) {
            throw new ApplicationException("Data Context was not initialized!");
        }
        return _dbContext;
    }
}
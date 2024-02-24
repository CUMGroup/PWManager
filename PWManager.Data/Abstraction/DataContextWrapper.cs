using PWManager.Data.Persistance;

namespace PWManager.Data.Abstraction; 

public class DataContextWrapper {

    public virtual bool DatabaseExists(string path) {
        return DataContext.DatabaseExists(path);
    }

    public virtual void InitDataContext(string path) {
        DataContext.InitDataContext(path);
    }

    internal virtual ApplicationDbContext GetDbContext() {
        return DataContext.GetDbContext();
    }
}
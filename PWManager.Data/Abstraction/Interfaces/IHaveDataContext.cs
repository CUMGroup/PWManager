using PWManager.Data.Persistance;

namespace PWManager.Data.Abstraction.Interfaces; 

internal interface IHaveDataContext {
    ApplicationDbContext GetDbContext();
}
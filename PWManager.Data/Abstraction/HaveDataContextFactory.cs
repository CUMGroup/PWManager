using PWManager.Data.Abstraction.Interfaces;

namespace PWManager.Data.Abstraction; 

internal static class HaveDataContextFactory {

    private static IHaveDataContext? _dataContext;

    internal static IHaveDataContext Create() {
        if (_dataContext is null) {
            throw new ApplicationException("Tried accessing the data context without initializing it first!");
        }
        return _dataContext;
    }

    internal static void Initialize(IHaveDataContext context) {
        if (_dataContext is not null) {
            throw new ApplicationException(
                "The data context was already set. The initialize method can only be called once!");
        }
        _dataContext = context;
    }
}
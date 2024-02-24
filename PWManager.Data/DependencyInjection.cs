using Microsoft.Extensions.DependencyInjection;
using PWManager.Data.Repositories;
using PWManager.Domain.Repositories;

namespace PWManager.Data; 

public static class DependencyInjection {


    public static IServiceCollection AddDataServices(this IServiceCollection services) {
        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ISettingsRepository, SettingsRepository>();
        return services;
    }
}
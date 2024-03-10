using Microsoft.Extensions.DependencyInjection;
using PWManager.Application.Services.Interfaces;
using PWManager.Data.Repositories;
using PWManager.Data.Services;
using PWManager.Data.System;
using PWManager.Domain.Repositories;

namespace PWManager.Data; 

public static class DependencyInjection {


    public static IServiceCollection AddDataServices(this IServiceCollection services) {
        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ISettingsRepository, SettingsRepository>();

        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IDatabaseInitializerService, DatabaseInitializerService>();
        
        services.AddTransient<IClipboard, Clipboard>();
        return services;
    }
}
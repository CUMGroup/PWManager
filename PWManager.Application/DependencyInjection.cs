using Microsoft.Extensions.DependencyInjection;
using PWManager.Application.Services;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Application; 

public static class DependencyInjection {

    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {

        services.AddTransient<ICryptService, CryptService>();
        services.AddTransient<IPasswordGeneratorService, PasswordGeneratorService>();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IGroupService, GroupService>();
        
        return services;
    }
    
}
using Microsoft.Extensions.DependencyInjection;
using PWManager.Application.Services;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Application; 

public static class DependencyInjection {

    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {

        services.AddTransient<IPasswordGeneratorService, PasswordGeneratorService>();
        
        return services;
    }
    
}
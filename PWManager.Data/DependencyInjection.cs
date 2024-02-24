using Microsoft.Extensions.DependencyInjection;

namespace PWManager.Data; 

public static class DependencyInjection {


    public static IServiceCollection AddDataServices(this IServiceCollection services) {
        return services;
    }
}
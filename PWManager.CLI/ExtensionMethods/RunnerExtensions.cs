using Microsoft.Extensions.DependencyInjection;
using PWManager.CLI.Controllers;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.ExtensionMethods; 

internal static class RunnerExtensions {

    public static void AddControllers(this IServiceCollection services) {
        services.AddTransient<HelpController>();
        services.AddTransient<InitController>();
    }
    public static void MapControllers(this ConsoleRunner runner) {
        
        runner.MapCommand<HelpController>(AvailableCommands.HELP);
        runner.MapCommand<InitController>(AvailableCommands.INIT);
    }
}
using Microsoft.Extensions.DependencyInjection;
using PWManager.CLI.Controllers;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.ExtensionMethods; 

internal static class RunnerExtensions {

    public static void AddControllers(this IServiceCollection services) {
        services.AddTransient<HelpController>();
        services.AddTransient<LoginController>();
        services.AddTransient<InitController>();
        services.AddTransient<ListController>();
        services.AddTransient<NewController>();
        services.AddTransient<GroupController>();
        services.AddTransient<GetController>();
        services.AddTransient<QuitController>();
    }
    public static void MapControllers(this ConsoleRunner runner) {
        
        runner.MapCommand<HelpController>(AvailableCommands.HELP);
        runner.MapCommand<LoginController>(AvailableCommands.LOGIN);
        runner.MapCommand<InitController>(AvailableCommands.INIT);
        runner.MapCommand<ListController>(AvailableCommands.LIST);
        runner.MapCommand<NewController>(AvailableCommands.NEW);
        runner.MapCommand<GroupController>(AvailableCommands.GROUP);
        runner.MapCommand<GetController>(AvailableCommands.GET);
        runner.MapCommand<QuitController>(AvailableCommands.QUIT);
    }
}
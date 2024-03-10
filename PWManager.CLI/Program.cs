using Microsoft.Extensions.DependencyInjection;
using PWManager.Application;
using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Environment;
using PWManager.CLI.ExtensionMethods;
using PWManager.CLI.Interfaces;
using PWManager.Data;
using PWManager.Data.System;

// Clean up hooks
var defaultColor = Console.ForegroundColor;
Console.CancelKeyPress += delegate {
    Console.ForegroundColor = defaultColor;
};

var services = new ServiceCollection();

// Configure Environment
var environment = new CliEnvironment();
services.AddSingleton<ICliEnvironment>(environment);
services.AddSingleton<ICryptEnvironment>(environment);
services.AddSingleton<IDebugEnvironment>(environment);
services.AddSingleton<IUserEnvironment>(environment);

// Add all services to DI
services.AddSingleton<IRunner, ConsoleRunner>();
// Add Layers
services.AddApplicationServices();
services.AddDataServices();

// Add Controllers
services.AddControllers();

var provider = services.BuildServiceProvider();

// Resolve the configured runner from the DI
var runner = provider.GetService<IRunner>();
ArgumentNullException.ThrowIfNull(runner);

// Map Controller routes
((ConsoleRunner)runner).MapControllers();

runner.Run(args);
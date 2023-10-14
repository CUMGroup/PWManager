using Microsoft.Extensions.DependencyInjection;
using PWManager.CLI;
using PWManager.CLI.Interfaces;

var services = new ServiceCollection();

// Add all services to DI
services.AddSingleton<IRunner, ConsoleRunner>();


var provider = services.BuildServiceProvider();

// Resolve the configured runner from the DI
var runner = provider.GetService<IRunner>();
ArgumentNullException.ThrowIfNull(runner);

runner.Run(args);
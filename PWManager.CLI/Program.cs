﻿using Microsoft.Extensions.DependencyInjection;
using PWManager.Application;
using PWManager.CLI;
using PWManager.CLI.Abstractions;
using PWManager.CLI.ExtensionMethods;
using PWManager.CLI.Interfaces;

var services = new ServiceCollection();

services.AddSingleton<CliEnvironment>();
// Add all services to DI
services.AddSingleton<IRunner, ConsoleRunner>();
// Add Layers
services.AddApplicationServices();

// Add Controllers
services.AddControllers();

var provider = services.BuildServiceProvider();

// Resolve the configured runner from the DI
var runner = provider.GetService<IRunner>();
ArgumentNullException.ThrowIfNull(runner);

// Map Controller routes
((ConsoleRunner)runner).MapControllers();

runner.Run(args);
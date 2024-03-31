using Microsoft.Extensions.DependencyInjection;
using PWManager.Application;
using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Environment;
using PWManager.CLI.ExtensionMethods;
using PWManager.CLI.Interfaces;
using PWManager.CLI.Services;
using PWManager.Data;
using Sharprompt;

// Clean up hooks
var defaultColor = Console.ForegroundColor;
Console.CancelKeyPress += delegate {
    Console.ForegroundColor = defaultColor;
};
Prompt.ThrowExceptionOnCancel = true;

var services = new ServiceCollection();

// Configure Environment
var environment = new CliEnvironment();
services.AddSingleton<ICliEnvironment>(environment);
services.AddSingleton<ICryptEnvironment>(environment);
services.AddSingleton<IDebugEnvironment>(environment);
services.AddSingleton<IUserEnvironment>(environment);
services.AddSingleton<ICancelEnvironment>(environment);

// Create Observer for key inputs to kick client for inactivity
var accountTimeOutObserver = new AccountTimeoutService(environment, environment);

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

#region Hosted Services
using var cancelTokenSource = new CancellationTokenSource();
var cancelToken = cancelTokenSource.Token;

accountTimeOutObserver.StartMonitoring(cancelToken);

var clipboard = provider.GetService<IClipboard>()!;
var clipboardTimeoutObserver = new ClipboardTimeoutService(clipboard, environment, cancelToken);
#endregion
try {
    runner.Run(args);
} finally {
    cancelTokenSource.Cancel();
    ConsoleInteraction.ResetConsole();
}

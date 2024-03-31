using PWManager.Application.Context;
using PWManager.CLI.Abstractions;

namespace PWManager.CLI.Services; 

public class AccountTimeoutService {

    private readonly IUserEnvironment _userEnv;
    private readonly ICancelEnvironment _cancelEnv;
    
    private DateTime _lastKeyInput;

    private Task? _monitoringTask;
    
    public AccountTimeoutService(IUserEnvironment userEnv, ICancelEnvironment cancelEnv) {
        _userEnv = userEnv;
        _cancelEnv = cancelEnv;
        ConsoleInteraction.OnConsoleInput += OnConsoleInput;
    }

    public void StartMonitoring(CancellationToken cancelToken) {
        if (_monitoringTask is not null) {
            return;
        }
        _lastKeyInput = DateTime.Now;

        _monitoringTask = TimeoutTask(cancelToken);
    }
    
    
    private async Task TimeoutTask(CancellationToken cancelToken) {
        try {
            while (DateTime.Now.Subtract(_lastKeyInput) <= GetAccountTimeoutSpan() || !_cancelEnv.CancelableState) {
                cancelToken.ThrowIfCancellationRequested();
                var timeoutSeconds = GetAccountTimeoutSpan().Seconds;
                
                var checkFrequencySeconds = MinMaxBetween(timeoutSeconds / 4, 15, 60);
                
                await Task.Delay(checkFrequencySeconds * 1000, cancelToken);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException) {
            return;
        }
        ConsoleInteraction.ResetConsole();
        PromptHelper.PrintColoredText(ConsoleColor.Magenta, UIstrings.KickedDueToInactivityFor(GetAccountTimeoutSpan().Minutes));
        System.Environment.Exit(0);
    }

    private TimeSpan GetAccountTimeoutSpan() {
        if (_userEnv.UserSettings is null) {
            return TimeSpan.FromMinutes(2);
        }
        return _userEnv.UserSettings.Timeout.AccountTimeOutDuration;
    }
    
    private int MinMaxBetween(int val, int min, int max) {
        return Math.Max(Math.Min(val, max), min);
    }
    
    private void OnConsoleInput() {
        _lastKeyInput = DateTime.Now;
    }

}
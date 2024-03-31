using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;

namespace PWManager.CLI.Services; 

public class ClipboardTimeoutService {

    private readonly IClipboard _clipboard;
    private readonly IUserEnvironment _userEnv;

    private readonly CancellationToken _programCancelToken;
    
    private CancellationTokenSource? _cancelTokenSource;
    
    public ClipboardTimeoutService(IClipboard clipboard, IUserEnvironment userEnv, CancellationToken cancellationToken) {
        _clipboard = clipboard;
        _userEnv = userEnv;
        _programCancelToken = cancellationToken;
        
        _clipboard.OnClipboardUpdated += OnClipboardUpdated;
    }

    private async Task ClipboardTimeoutTask(CancellationToken cancelToken) {
        try {
            cancelToken.ThrowIfCancellationRequested();
            await Task.Delay(GetClipboardTimeoutSpan(), cancelToken);
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException) {
            return;
        }
        
        _clipboard.ClearClipboard();
    }
    
    
    private TimeSpan GetClipboardTimeoutSpan() {
        if (_userEnv.UserSettings is null) {
            return TimeSpan.FromSeconds(30);
        }

        return _userEnv.UserSettings.Timeout.ClipboardTimeOutDuration;
    }
    
    private void OnClipboardUpdated(string val) {
        if (string.IsNullOrWhiteSpace(val)) {
            return;
        }
        
        _cancelTokenSource?.Cancel();
        _cancelTokenSource?.Dispose();

        _cancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_programCancelToken);
        _ = ClipboardTimeoutTask(_cancelTokenSource.Token);
    }
}
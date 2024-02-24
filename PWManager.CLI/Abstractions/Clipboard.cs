using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;

namespace PWManager.CLI.Abstractions; 

public class Clipboard : IClipboard {
    
    public void WriteClipboard(string val) {
        if (OperatingSystem.IsWindows()) {
            var escaped = val.Replace("\n", "\\n").Replace("\"", "\"\"");
            $"echo \"{escaped}\" | Set-Clipboard"
                .PowerShell();
        }
        else if(OperatingSystem.IsMacOS()) {
            var escaped = val.Replace("\n", "\\n").Replace("\"", "\\\"");
            $"echo -n \"{escaped}\" | pbcopy"
                .Bash();
        }else if (OperatingSystem.IsLinux()) {
            var escaped = val.Replace("\n", "\\n").Replace("\"", "\\\"");
            $"echo -n \"{escaped}\" | xclip -selection c"
                .Bash();
        }
        else {
            throw new UserFeedbackException("Your Operating System does not support the clipboard functionality");
        }
    }

    public void ClearClipboard() {
        WriteClipboard(" ");
    }
}
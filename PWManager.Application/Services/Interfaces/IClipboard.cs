namespace PWManager.Application.Services.Interfaces; 

public interface IClipboard {

    event Action<string> OnClipboardUpdated;
    
    void WriteClipboard(string val);

    void ClearClipboard();
}
namespace PWManager.Application.Services.Interfaces; 

public interface IClipboard {

    void WriteClipboard(string val);

    void ClearClipboard();
}
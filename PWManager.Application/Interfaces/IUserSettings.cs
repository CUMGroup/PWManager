using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Interfaces; 

public interface IUserSettings {

    public PasswordGeneratorCriteria GetPasswordGeneratorCriteria();
    public ClipboardTimeoutSetting GetClipboardTimeoutSetting();
    public MainGroupSetting GetMainGroupSetting();
}

using PWManager.Domain.Entities;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services.Interfaces;
public interface ISettingsService {

    void ChangePasswordGenerationCriteria(PasswordGeneratorCriteria generatorCriteria);
    void ChangeClipboardTimeoutSetting(ClipboardTimeoutSetting clipboardTimeoutSetting);
    void ChangeMainGroupSetting(MainGroupSetting mainGroupSetting);
}

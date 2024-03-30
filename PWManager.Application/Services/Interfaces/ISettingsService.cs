
using PWManager.Domain.Entities;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services.Interfaces;
public interface ISettingsService {

    void ChangePasswordGenerationCriteria(PasswordGeneratorCriteria generatorCriteria);
    void ChangeClipboardTimeoutSetting(TimeSpan timeout);
    void ChangeAccountTimeoutSetting(TimeSpan timeout);
    void ChangeMainGroupSetting(MainGroupSetting mainGroupSetting);
    Settings GetSettings();
}


using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services;
public class SettingsService : ISettingsService {

    private readonly ISettingsRepository _settingsRepository;
    private readonly IUserEnvironment _userEnv;
    
    public SettingsService(ISettingsRepository settingsRepository, IUserEnvironment userEnv) {
        _settingsRepository = settingsRepository;
        _userEnv = userEnv;
    }

    public void ChangeClipboardTimeoutSetting(TimeSpan timeout) {
        var settings = GetSettings();
        settings.Timeout = new TimeoutSettings(timeout, settings.Timeout.AccountTimeOutDuration);
        _settingsRepository.UpdateSettings(settings);
    }
    
    public void ChangeAccountTimeoutSetting(TimeSpan timeout) {
        var settings = GetSettings();
        settings.Timeout = new TimeoutSettings(settings.Timeout.ClipboardTimeOutDuration, timeout);
        _settingsRepository.UpdateSettings(settings);
    }

    public void ChangeMainGroupSetting(MainGroupSetting mainGroup) {
        var settings = GetSettings();
        settings.MainGroup = mainGroup;
        _settingsRepository.UpdateSettings(settings);
    }

    public void ChangePasswordGenerationCriteria(PasswordGeneratorCriteria generatorCriteria) {
        var settings = GetSettings();
        settings.PwGenCriteria = generatorCriteria;
        _settingsRepository.UpdateSettings(settings);
    }

    public Settings GetSettings() {
        return _userEnv.UserSettings ??= _settingsRepository.GetSettings();
    }
}
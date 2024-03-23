
using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services;
public class SettingsService : ISettingsService {

    private readonly ISettingsRepository _settingsRepository;

    public SettingsService(ISettingsRepository settingsRepository) {
        _settingsRepository = settingsRepository;
    }

    public void ChangeClipboardTimeoutSetting(ClipboardTimeoutSetting clipboardTimeout) {
        var settings = _settingsRepository.GetSettings();
        settings.ClipboardTimeout = clipboardTimeout;
        _settingsRepository.UpdateSettings(settings);
    }

    public void ChangeMainGroupSetting(MainGroupSetting mainGroup) {
        var settings = _settingsRepository.GetSettings();
        settings.MainGroup = mainGroup;
        _settingsRepository.UpdateSettings(settings);
    }

    public void ChangePasswordGenerationCriteria(PasswordGeneratorCriteria generatorCriteria) {
        var settings = _settingsRepository.GetSettings();
        settings.PwGenCriteria = generatorCriteria;
        _settingsRepository.UpdateSettings(settings);
    }

    public Settings GetSettings() {
        return _settingsRepository.GetSettings();
    }
}
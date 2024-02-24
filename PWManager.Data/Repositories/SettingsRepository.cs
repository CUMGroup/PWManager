using PWManager.Application.Context;
using PWManager.Data.Models;
using PWManager.Data.Persistance;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;
using PWManager.Domain.ValueObjects;

namespace PWManager.Data.Repositories; 

internal class SettingsRepository : ISettingsRepository {

    private readonly IApplicationEnvironment _environment;
    private ApplicationDbContext _dbContext => DataContext.GetDbContext();
    private readonly ICryptService _cryptService;
    
    public SettingsRepository(IApplicationEnvironment env, ICryptService cryptService) {
        _environment = env;
        _cryptService = cryptService;
    }
    
    public Settings GetSettings() {
        var settingsModel = _dbContext.Settings.First(e => e.UserId == _environment.CurrentUser.Id);
        if (settingsModel is null) {
            settingsModel = new SettingsModel {
                UserId = _environment.CurrentUser.Id,
                Id = Guid.NewGuid().ToString(),
                MainGroupIdentifier = _cryptService.Encrypt("main")
            };
            UpdateSettings(settingsModel);
        }

        return SettingsModelToEntity(settingsModel);
    }

    public bool UpdateSettings(Settings settings) {
        return UpdateSettings(SettingsEntityToModel(settings));
    }

    private bool UpdateSettings(SettingsModel model) {
        _dbContext.Settings.Update(model);
        return _dbContext.SaveChanges() > 0;
    }

    private Settings SettingsModelToEntity(SettingsModel e) {
        return new Settings(
            e.Id,
            e.Created,
            e.Updated,
            e.UserId,
            new PasswordGeneratorCriteria(e.IncludeLowerCase, e.IncludeUpperCase, e.IncludeNumeric, e.IncludeSpecial,
                e.IncludeBrackets, e.IncludeSpaces, e.MinLength, e.MaxLength),
            new ClipboardTimeoutSetting(e.TimeOutDuration),
            new MainGroupSetting(_cryptService.Decrypt(e.MainGroupIdentifier))
        );
    }

    private SettingsModel SettingsEntityToModel(Settings e) {
        return new SettingsModel {
            Id = e.Id,
            Created = e.Created,
            Updated = e.Updated,
            UserId = _environment.CurrentUser.Id,
            IncludeLowerCase = e.PwGenCriteria.IncludeLowerCase,
            IncludeUpperCase = e.PwGenCriteria.IncludeUpperCase,
            IncludeSpaces = e.PwGenCriteria.IncludeSpaces,
            IncludeBrackets = e.PwGenCriteria.IncludeBrackets,
            IncludeNumeric = e.PwGenCriteria.IncludeNumeric,
            IncludeSpecial = e.PwGenCriteria.IncludeSpecial,
            MinLength = e.PwGenCriteria.MinLength,
            MaxLength = e.PwGenCriteria.MaxLength,
            TimeOutDuration = e.ClipboardTimeout.TimeOutDuration,
            MainGroupIdentifier = e.MainGroup.MainGroupIdentifier,
        };
    }
}
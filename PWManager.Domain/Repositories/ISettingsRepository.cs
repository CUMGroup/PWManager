using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    internal interface ISettingsRepository {
        public Settings GetSettingsFor(string userId);
        public bool UpdateSettings(Settings settings);
    }
}

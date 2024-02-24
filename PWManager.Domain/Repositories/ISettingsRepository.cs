using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    public interface ISettingsRepository {
        public Settings GetSettings();
        public bool UpdateSettings(Settings settings);
    }
}
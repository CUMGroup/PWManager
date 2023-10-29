using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    public interface ISettingsRepository {
        public Settings GetSettingsFor(string userId);
        public bool UpdateSettings(Settings settings);
    }
}
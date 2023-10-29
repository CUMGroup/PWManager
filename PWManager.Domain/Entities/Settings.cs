
using PWManager.Domain.Common;
using PWManager.Domain.ValueObjects;

namespace PWManager.Domain.Entities {
    public class Settings : Entity {
        public string UserID { get; set; }
        public PasswordGeneratorCriteria PWGenCriteria { get; set; }
        public ClipboardTimeoutSetting ClipboardTimeout { get; set; }
        public MainGroupSetting MainGroup { get; set; }

        public Settings(string userId, PasswordGeneratorCriteria pwGenCriteria, ClipboardTimeoutSetting clipboardTimeout, MainGroupSetting mainGroup) : base(){
            UserID = userId;
            PWGenCriteria = pwGenCriteria;
            ClipboardTimeout = clipboardTimeout;
            MainGroup = mainGroup;
        }
        public Settings(string id, DateTimeOffset created, DateTimeOffset updated, string userId, PasswordGeneratorCriteria pwGenCriteria, ClipboardTimeoutSetting clipboardTimeout, MainGroupSetting mainGroup) : base(id, created, updated) {
            UserID = userId;
            PWGenCriteria = pwGenCriteria;
            ClipboardTimeout = clipboardTimeout;
            MainGroup = mainGroup;
        }
    }
}


using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    public class ClipboardTimeoutSetting : ValueObject {

        public TimeSpan TimeOutDuration { get; }

        public ClipboardTimeoutSetting(TimeSpan timeOutDuration) {
            TimeOutDuration = timeOutDuration;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return TimeOutDuration;
        }
    }
}

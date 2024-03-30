
using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    public class TimeoutSettings : ValueObject {

        public TimeSpan ClipboardTimeOutDuration { get; }
        public TimeSpan AccountTimeOutDuration { get; }

        public TimeoutSettings(TimeSpan clipboardTimeOutDuration, TimeSpan accountTimeOutDuration) {
            ClipboardTimeOutDuration = clipboardTimeOutDuration;
            AccountTimeOutDuration = accountTimeOutDuration;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return ClipboardTimeOutDuration;
            yield return AccountTimeOutDuration;
        }
    }
}

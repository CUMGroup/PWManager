
using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    internal class MainGroupSetting : ValueObject {
        public string MainGroupIdentifier { get; }

        public MainGroupSetting(string mainGroupIdentifier) {
            MainGroupIdentifier = mainGroupIdentifier;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return MainGroupIdentifier;
        }
    }
}

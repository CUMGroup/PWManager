

using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    public class Password : ValueObject {

        private readonly string _passwordHash;
        private readonly uint _strength;

        public string PasswordHash { get => _passwordHash; set => new Password(value, Strength); }
        public uint Strength { get => _strength; set => new Password(PasswordHash, value); }

        public Password(string passwordHash, uint strength) {
            _passwordHash = passwordHash;
            _strength = strength;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return _passwordHash;
            yield return _strength;
        }
    }
}

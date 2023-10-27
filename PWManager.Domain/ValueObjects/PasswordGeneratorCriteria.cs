
using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    public class PasswordGeneratorCriteria : ValueObject {

        private readonly bool _includeLowerCase;
        private readonly bool _includeUpperCase;
        private readonly bool _includeNumeric;
        private readonly bool _includeSpecial;
        private readonly bool _includeSpaces;
        private readonly bool _includeBrackets;

        private readonly uint _minLength;
        private readonly uint _maxLength;

        public bool IncludeLowerCase { get => _includeLowerCase; set => new PasswordGeneratorCriteria(value, IncludeUpperCase, IncludeNumeric, IncludeSpecial, IncludeBrackets, IncludeSpaces, MinLength, MaxLength); }
        public bool IncludeUpperCase { get => _includeUpperCase; set => new PasswordGeneratorCriteria(IncludeLowerCase, value, IncludeNumeric, IncludeSpecial, IncludeBrackets, IncludeSpaces, MinLength, MaxLength); }
        public bool IncludeNumeric { get => _includeNumeric; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, value, IncludeSpecial, IncludeBrackets, IncludeSpaces, MinLength, MaxLength); }
        public bool IncludeSpecial { get => _includeSpecial; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, IncludeNumeric, value, IncludeBrackets, IncludeSpaces, MinLength, MaxLength); }
        public bool IncludeSpaces { get => _includeSpaces; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, IncludeNumeric, IncludeSpecial, value, IncludeSpaces, MinLength, MaxLength); }
        public bool IncludeBrackets { get => _includeBrackets; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, IncludeNumeric, IncludeSpecial, IncludeBrackets, value, MinLength, MaxLength); }
        public uint MinLength { get => _minLength; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, IncludeNumeric, IncludeSpecial, IncludeBrackets, IncludeSpaces, value, MaxLength); }
        public uint MaxLength { get => _maxLength; set => new PasswordGeneratorCriteria(IncludeLowerCase, IncludeUpperCase, IncludeNumeric, IncludeSpecial, IncludeBrackets, IncludeSpaces, MinLength, value); }

        public PasswordGeneratorCriteria(bool includeLowerCase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeBrackets, bool includeSpaces, uint minLength, uint maxLength)
        {
            _includeLowerCase = includeLowerCase;
            _includeUpperCase = includeUppercase;
            _includeNumeric = includeNumeric;
            _includeSpecial = includeSpecial;
            _includeBrackets = includeBrackets;
            _includeSpaces = includeSpaces;
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return _includeLowerCase;
            yield return _includeUpperCase;
            yield return _includeNumeric;
            yield return _includeSpecial;
            yield return _includeBrackets;
            yield return _includeSpaces;
            yield return _minLength;
            yield return _maxLength;
        }
    }
}

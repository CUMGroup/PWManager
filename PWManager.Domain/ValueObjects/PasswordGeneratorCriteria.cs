
using PWManager.Domain.Common;

namespace PWManager.Domain.ValueObjects {
    public class PasswordGeneratorCriteria : ValueObject {

        public bool IncludeLowerCase { get; }
        public bool IncludeUpperCase { get; }
        public bool IncludeNumeric { get; }
        public bool IncludeSpecial { get; }
        public bool IncludeSpaces { get; }
        public bool IncludeBrackets { get; }
        public int MinLength { get; }
        public int MaxLength { get; }
        
        public PasswordGeneratorCriteria(bool includeLowerCase, bool includeUpperCase, bool includeNumeric, bool includeSpecial, bool includeBrackets, bool includeSpaces, int minLength, int maxLength) {
            if (minLength <= 0) {
                throw new ArgumentException("MinLength cannot be less than or equal to 0");
            }

            if (maxLength < minLength) {
                throw new ArgumentException("MaxLength cannot be less than MinLength");
            }
            
            IncludeLowerCase = includeLowerCase;
            IncludeUpperCase = includeUpperCase;
            IncludeNumeric = includeNumeric;
            IncludeSpecial = includeSpecial;
            IncludeBrackets = includeBrackets;
            IncludeSpaces = includeSpaces;
            MinLength = minLength;
            MaxLength = maxLength;
            
            if (NoCharactersIncluded()) {
                throw new ArgumentException("Password Generator must have some characters enabled!");
            }
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return IncludeLowerCase;
            yield return IncludeUpperCase;
            yield return IncludeNumeric;
            yield return IncludeSpecial;
            yield return IncludeBrackets;
            yield return IncludeSpaces;
            yield return MinLength;
            yield return MaxLength;
        }
        
        private bool NoCharactersIncluded() {
            return !IncludeLowerCase && !IncludeUpperCase && !IncludeNumeric && !IncludeSpecial && !IncludeBrackets;
        }
    }
}

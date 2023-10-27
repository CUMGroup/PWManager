
namespace PWManager.Domain.Common {
    public abstract class ValueObject {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj) {
            if (obj is null || obj.GetType() != this.GetType()) {
                return false;
            }

            var otherObj = (ValueObject)obj; 
            return GetEqualityComponents().SequenceEqual(otherObj.GetEqualityComponents());
        }

        public override int GetHashCode() {
            return GetEqualityComponents()
                .Select(x => x is not null ? x.GetHashCode() : 0 )
                .Aggregate((x, y) => x ^ y);
        }
    }
}

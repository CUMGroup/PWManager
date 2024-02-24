using System.Runtime.Serialization;

namespace PWManager.Domain.Exceptions {
    public class SecurePropertiesException : Exception {
        public SecurePropertiesException() { }
        protected SecurePropertiesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public SecurePropertiesException(string? message) : base(message) { }
        public SecurePropertiesException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}

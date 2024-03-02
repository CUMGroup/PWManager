using System.Runtime.Serialization;

namespace PWManager.Application.Exceptions; 

public class LoginException : Exception {
    public LoginException() { }
    protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public LoginException(string? message) : base(message) { }
    public LoginException(string? message, Exception? innerException) : base(message, innerException) { }
}
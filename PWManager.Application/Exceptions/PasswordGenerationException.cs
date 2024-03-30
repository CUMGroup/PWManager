using System.Runtime.Serialization;

namespace PWManager.Application.Exceptions; 

public class PasswordGenerationException : UserFeedbackException {
    public PasswordGenerationException() { }
    protected PasswordGenerationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public PasswordGenerationException(string? message) : base(message) { }
    public PasswordGenerationException(string? message, Exception? innerException) : base(message, innerException) { }
}
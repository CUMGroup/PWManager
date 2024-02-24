using System.Runtime.Serialization;

namespace PWManager.Application.Exceptions; 

public class UserFeedbackException : Exception {
    public UserFeedbackException() { }
    protected UserFeedbackException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public UserFeedbackException(string? message) : base(message) { }
    public UserFeedbackException(string? message, Exception? innerException) : base(message, innerException) { }
}
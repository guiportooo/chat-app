namespace ChatApp.Api.Domain.Exceptions
{
    public class InvalidChatCommandException : DomainException
    {
        public InvalidChatCommandException(string command) : base($"Chat command {command} is invalid")
        {
        }
    }
}
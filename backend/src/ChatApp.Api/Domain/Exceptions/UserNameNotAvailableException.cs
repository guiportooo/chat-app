namespace ChatApp.Api.Domain.Exceptions
{
    public class UserNameNotAvailableException : DomainException
    {
        public UserNameNotAvailableException(string userName) : base($"Username {userName} not available")
        {
        }
    }
}
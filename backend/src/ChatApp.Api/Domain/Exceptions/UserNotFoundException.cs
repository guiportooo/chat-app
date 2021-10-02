namespace ChatApp.Api.Domain.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string userName) : base($"User {userName} does not exist")
        {
        }
    }
}
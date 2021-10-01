namespace ChatApp.Api.Domain.Models
{
    public class User
    {
        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}
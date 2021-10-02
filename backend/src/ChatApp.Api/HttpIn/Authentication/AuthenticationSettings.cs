namespace ChatApp.Api.HttpIn.Authentication
{
    public class AuthenticationSettings
    {
        public const string Name = "Authentication";

        public AuthenticationSettings(string secret, int hoursToExpire)
        {
            Secret = secret;
            HoursToExpire = hoursToExpire;
        }

        public AuthenticationSettings()
        {
        }

        public string Secret { get; set; } = null!;
        public int HoursToExpire { get; set; }
    }
}
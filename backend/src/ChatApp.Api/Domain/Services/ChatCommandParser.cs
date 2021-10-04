namespace ChatApp.Api.Domain.Services
{
    using System.Text.RegularExpressions;

    public interface IChatCommandParser
    {
        (string command, string value) Parse(string text);
    }

    public class ChatCommandParser : IChatCommandParser
    {
        public (string, string) Parse(string text)
        {
            if (!IsCommand(text))
                return ("", "");

            var index = text.IndexOf('=');
            return (text[..index], text[^(text.Length - index - 1)..]);
        }

        private static bool IsCommand(string text) => new Regex("/[a-zA-Z]+=.+", RegexOptions.IgnoreCase).IsMatch(text);
    }
}
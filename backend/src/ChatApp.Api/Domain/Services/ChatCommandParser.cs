namespace ChatApp.Api.Domain.Services
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public interface IChatCommandParser
    {
        bool IsCommand(string text);
        (string command, string value) Parse(string text);
    }

    public class ChatCommandParser : IChatCommandParser
    {
        public bool IsCommand(string text) => new Regex("/[a-zA-Z]+=.+", RegexOptions.IgnoreCase).IsMatch(text);

        public (string, string) Parse(string text)
        {
            if (!IsCommand(text))
                return ("", "");

            var index = text.IndexOf('=');
            var command = text.Length - index - 2;
            var value = text.Length - index - 1;
            return (text[..command], text[^value..]);
        }
    }
}
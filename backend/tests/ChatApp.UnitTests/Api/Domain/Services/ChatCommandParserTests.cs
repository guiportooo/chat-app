namespace ChatApp.UnitTests.Api.Domain.Services
{
    using ChatApp.Api.Domain.Services;
    using FluentAssertions;
    using NUnit.Framework;

    public class ChatCommandParserTests
    {
        private ChatCommandParser _parser;

        [SetUp]
        public void Setup() => _parser = new ChatCommandParser();

        [TestCase("/stock=APPL.US", true)]
        [TestCase("/stock=APPL=US", true)]
        [TestCase("/stock=APPL/US", true)]
        [TestCase("stock=APPL.US", false)]
        [TestCase("/stock>APPL.US", false)]
        public void Should_identify_valid_command(string text, bool isCommand) =>
            _parser.IsCommand(text).Should().Be(isCommand);

        [TestCase("/stock=APPL.US", "/stock", "APPL.US")]
        [TestCase("/stock=APPL=US", "/stock", "APPL=US")]
        [TestCase("/stock=APPL/US", "/stock", "APPL/US")]
        [TestCase("/stock=APPL.USY", "/stock", "APPL.USY")]
        [TestCase("/stock=APPLUS", "/stock", "APPLUS")]
        [TestCase("/stock=APPLU", "/stock", "APPLU")]
        [TestCase("stock=APPL.US", "", "")]
        [TestCase("/stock>APPL.US", "", "")]
        public void Should_parse_valid_command(string text, string command, string value) =>
            _parser.Parse(text).Should().Be((command, value));
    }
}
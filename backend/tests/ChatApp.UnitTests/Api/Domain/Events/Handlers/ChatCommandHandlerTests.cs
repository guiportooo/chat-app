namespace ChatApp.UnitTests.Api.Domain.Events.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.Events;
    using ChatApp.Api.Domain.Events.Handlers;
    using ChatApp.Api.Domain.Exceptions;
    using ChatApp.Api.Domain.IntegrationEvents;
    using ChatApp.Api.Domain.IntegrationEvents.Publishers;
    using ChatApp.Api.Domain.Services;
    using FluentAssertions;
    using Moq;
    using Moq.AutoMock;
    using NUnit.Framework;

    public class ChatCommandHandlerTests
    {
        private AutoMocker _mocker;
        private ChatCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<ChatCommandHandler>();
        }

        [Test]
        public async Task Should_ignore_when_message_sent_is_not_a_command()
        {
            var messageSent = new AutoFaker<MessageSent>().Generate();

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.IsCommand(messageSent.Text))
                .Returns(false);

            await _handler.Handle(messageSent, CancellationToken.None);

            _mocker
                .GetMock<IStockQuoteRequestedPublisher>()
                .Verify(x => x.Publish(It.IsAny<StockQuoteRequested>()), Times.Never);
        }

        [Test]
        public async Task Should_throw_exception_when_command_is_invalid()
        {
            var messageSent = new AutoFaker<MessageSent>().Generate();

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.IsCommand(messageSent.Text))
                .Returns(true);

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.Parse(messageSent.Text))
                .Returns(("", ""));

            Func<Task> handle = async () => await _handler.Handle(messageSent, CancellationToken.None);

            handle.Should().Throw<InvalidChatCommandException>()
                .WithMessage($"Chat command {messageSent.Text} is invalid");

            _mocker
                .GetMock<IStockQuoteRequestedPublisher>()
                .Verify(x => x.Publish(It.IsAny<StockQuoteRequested>()), Times.Never);
        }

        [Test]
        public async Task Should_publish_stock_quote_command()
        {
            const string command = "/stock";
            const string value = "APPL.US";
            var text = $"{command}={value}";
            const string roomCode = "general";

            var messageSent = new AutoFaker<MessageSent>()
                .RuleFor(x => x.Text, () => text)
                .RuleFor(x => x.RoomCode, () => roomCode)
                .Generate();

            var commandPublished = new StockQuoteRequested("", "");
            var expectedCommandPublished = new StockQuoteRequested(value, roomCode);

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.IsCommand(text))
                .Returns(true);

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.Parse(text))
                .Returns((command, value));

            _mocker
                .GetMock<IStockQuoteRequestedPublisher>()
                .Setup(x => x.Publish(It.IsAny<StockQuoteRequested>()))
                .Callback<StockQuoteRequested>(actualCommand => commandPublished = actualCommand);

            await _handler.Handle(messageSent, CancellationToken.None);

            commandPublished.Should().BeEquivalentTo(expectedCommandPublished);
        }
    }
}
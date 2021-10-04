namespace ChatApp.UnitTests.Api.Domain.EventHandlers.StockBot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.EventHandlers.StockBot;
    using ChatApp.Api.Domain.Events;
    using ChatApp.Api.Domain.Exceptions;
    using ChatApp.Api.Domain.IntegrationEvents.Publishers;
    using ChatApp.Api.Domain.Services;
    using FluentAssertions;
    using Moq;
    using Moq.AutoMock;
    using NUnit.Framework;

    public class MessageSentHandlerTests
    {
        private AutoMocker _mocker;
        private MessageSentHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<MessageSentHandler>();
        }

        [Test]
        public async Task Should_not_publish_command_when_message_is_not_a_command()
        {
            var messageSent = new AutoFaker<MessageSent>()
                .RuleFor(x => x.Text, () => "not.a.command")
                .Generate();

            _mocker
                .GetMock<IChatCommandParser>()
                .Setup(x => x.Parse(messageSent.Text))
                .Returns(("", ""));

            await _handler.Handle(messageSent, CancellationToken.None);

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
namespace ChatApp.UnitTests.Api.Domain.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoBogus;
    using ChatApp.Api.Domain.Commands;
    using ChatApp.Api.Domain.Models;
    using ChatApp.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using Moq.AutoMock;
    using NUnit.Framework;

    public class SendMessageHandlerTests
    {
        private AutoMocker _mocker;
        private SendMessageHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<SendMessageHandler>();
        }

        [Test]
        public async Task Should_not_save_message_to_the_database_when_message_is_a_command()
        {
            var command = new AutoFaker<SendMessage>()
                .RuleFor(x => x.Text, () => "/stock=APPL.US")
                .Generate();
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>().Generate();

            _mocker
                .GetMock<IRoomRepository>()
                .Setup(x => x.GetByCode(command.RoomCode))
                .ReturnsAsync(room);

            _mocker
                .GetMock<IUserRepository>()
                .Setup(x => x.GetByUserName(command.UserName))
                .ReturnsAsync(user);

            var message = await _handler.Handle(command, CancellationToken.None);

            message.Should().BeNull();

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(x => x.Add(It.IsAny<Message>()), Times.Never);
        }

        [Test]
        public async Task Should_not_save_message_to_the_database_when_user_is_stock_bot()
        {
            var command = new AutoFaker<SendMessage>()
                .RuleFor(x => x.UserName, () => StockBotUser.BotUserName)
                .Generate();
            var room = new AutoFaker<Room>().Generate();

            _mocker
                .GetMock<IRoomRepository>()
                .Setup(x => x.GetByCode(command.RoomCode))
                .ReturnsAsync(room);

            var message = await _handler.Handle(command, CancellationToken.None);

            message.Should().BeNull();

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(x => x.Add(It.IsAny<Message>()), Times.Never);
        }

        [Test]
        public async Task Should_save_message_to_the_database_when_message_is_not_a_command_and_user_is_not_stock_bot()
        {
            var command = new AutoFaker<SendMessage>().Generate();
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>()
                .RuleFor(x => x.UserName, () => "not.a.bot")
                .Generate();

            _mocker
                .GetMock<IRoomRepository>()
                .Setup(x => x.GetByCode(command.RoomCode))
                .ReturnsAsync(room);

            _mocker
                .GetMock<IUserRepository>()
                .Setup(x => x.GetByUserName(command.UserName))
                .ReturnsAsync(user);

            var message = await _handler.Handle(command, CancellationToken.None);

            var expectedMessage = new Message(command.Text, room, user);

            message.Should().BeEquivalentTo(expectedMessage, opt =>
                opt
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                    .WhenTypeIs<DateTime>());

            _mocker
                .GetMock<IMessageRepository>()
                .Verify(x => x.Add(message), Times.Once);
        }
    }
}
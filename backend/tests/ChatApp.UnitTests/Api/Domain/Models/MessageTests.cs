namespace ChatApp.UnitTests.Api.Domain.Models
{
    using System;
    using AutoBogus;
    using ChatApp.Api.Domain.Models;
    using FluentAssertions;
    using NUnit.Framework;

    public class MessageTests
    {
        [Test]
        public void Should_create_message_with_timestamp()
        {
            const string text = "Hello World!";
            const int roomId = 123;
            const int userId = 321;

            var message = new Message(text, roomId, userId);

            message.Text.Should().Be(text);
            message.RoomId.Should().Be(roomId);
            message.UserId.Should().Be(userId);
            message.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Test]
        public void Should_create_message_when_user_is_not_bot()
        {
            const string text = "Hello World!";
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>()
                .RuleFor(x => x.UserName, () => "not.a.bot")
                .Generate();

            var message = new Message(text, room, user);

            message.Text.Should().Be(text);
            message.RoomId.Should().Be(room.Id);
            message.UserId.Should().Be(user.Id);
            message.User.Should().BeNull();
            message.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Test]
        public void Should_create_message_when_user_is_bot()
        {
            const string text = "Hello World!";
            var room = new AutoFaker<Room>().Generate();
            var user = new StockBotUser();

            var message = new Message(text, room, user);

            message.Text.Should().Be(text);
            message.RoomId.Should().Be(room.Id);
            message.UserId.Should().Be(0);
            message.User.Should().Be(user);
            message.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }
    }
}
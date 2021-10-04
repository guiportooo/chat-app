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
            message.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
        }

        [Test]
        public void Should_create_message_when_user_is_not_bot()
        {
            const string text = "Hello World!";
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>().Generate();

            var message = new Message(text, room, user);

            message.Text.Should().Be(text);
            message.User.Should().BeEquivalentTo(user);
            message.Room.Should().BeEquivalentTo(room);
            message.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
        }

        [TestCase("/stock=APPL.US", false)]
        [TestCase("/stock=APPL=US", false)]
        [TestCase("/stock=APPL/US", false)]
        [TestCase("stock=APPL.US", true)]
        [TestCase("/stock>APPL.US", true)]
        public void Should_not_be_saved_when_is_a_command_and_user_is_not_a_bot(string text, bool shouldBeSaved)
        {
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>()
                .RuleFor(x => x.UserName, () => "not.a.bot")
                .Generate();

            var message = new Message(text, room, user);

            message.ShouldBeSaved.Should().Be(shouldBeSaved);
        }

        [Test]
        public void Should_not_be_saved_when_is_not_a_command_and_user_is_a_bot()
        {
            const string text = "not.a.command";
            var room = new AutoFaker<Room>().Generate();
            var user = new StockBotUser();

            var message = new Message(text, room, user);

            message.ShouldBeSaved.Should().Be(false);
        }

        [Test]
        public void Should_be_saved_when_is_not_a_command_and_user_is_not_a_bot()
        {
            const string text = "not.a.command";
            var room = new AutoFaker<Room>().Generate();
            var user = new AutoFaker<User>()
                .RuleFor(x => x.UserName, () => "not.a.bot")
                .Generate();

            var message = new Message(text, room, user);

            message.ShouldBeSaved.Should().Be(true);
        }
    }
}
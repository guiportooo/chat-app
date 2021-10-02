namespace ChatApp.UnitTests.Api.Domain.Models
{
    using System;
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
    }
}
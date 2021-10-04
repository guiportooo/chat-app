namespace ChatApp.Api.Domain.Models
{
    using System;

    public class Message
    {
        public Message(string text, int roomId, int userId)
        {
            Timestamp = DateTime.UtcNow;
            Text = text;
            RoomId = roomId;
            UserId = userId;
        }

        public Message(string text, Room room, User user)
        {
            Timestamp = DateTime.UtcNow;
            Text = text;
            RoomId = room.Id;

            if (user.IsBot) User = user;
            else UserId = user.Id;
        }

        public int Id { get; private set; }
        public string Text { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int RoomId { get; private set; }
        public Room Room { get; private set; } = null!;
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
    }
}
namespace ChatApp.Api.Domain.Models
{
    using System;
    using System.Text.RegularExpressions;

    public class Message
    {
        public Message(string text, int roomId, int userId)
        {
            Timestamp = DateTime.Now;
            Text = text;
            RoomId = roomId;
            UserId = userId;
        }

        public Message(string text, Room room, User user)
        {
            Timestamp = DateTime.Now;
            Text = text;
            Room = room;
            User = user;
        }

        public int Id { get; private set; }
        public string Text { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int RoomId { get; private set; }
        public Room Room { get; private set; } = null!;
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public bool ShouldBeSaved => !IsCommand && !User.IsBot;
        private bool IsCommand => new Regex("/[a-zA-Z]+=.+", RegexOptions.IgnoreCase).IsMatch(Text);
    }
}
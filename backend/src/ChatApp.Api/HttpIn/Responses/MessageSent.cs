namespace ChatApp.Api.HttpIn.Responses
{
    using System;

    public class MessageSent
    {
        public MessageSent(int id, string text, DateTime timestamp)
        {
            Id = id;
            Text = text;
            Timestamp = timestamp;
        }

        public int Id { get; }
        public string Text { get; }
        public DateTime Timestamp { get; }
        public string UserName { get; set; } = null!;
        public string RoomCode { get; set; } = null!;
    }
}
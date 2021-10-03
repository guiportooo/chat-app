namespace ChatApp.Api.HttpIn.Responses
{
    using System.Collections.Generic;
    using System.Linq;
    using Models = Domain.Models;

    public class MessagesSent
    {
        public IEnumerable<MessageSent> Messages { get; set; } = new List<MessageSent>();
        
        public MessagesSent()
        {
        }

        public MessagesSent(IEnumerable<Models.Message> messages)
        {
            Messages = messages
                .Select(x => new MessageSent(x.Id, x.Text, x.Timestamp)
                {
                    UserName = x.User.UserName,
                    RoomCode = x.Room.Code
                })
                .OrderByDescending(x => x.Timestamp);
        }
    }
}
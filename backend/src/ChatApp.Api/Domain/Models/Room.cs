namespace ChatApp.Api.Domain.Models
{
    using System.Collections.Generic;

    public class Room
    {
        public Room(string code)
        {
            Code = code;
            Messages = new HashSet<Message>();
        }

        public int Id { get; private set; }
        public string Code { get; private set; }
        public virtual ICollection<Message> Messages { get; private set; }
    }
}
namespace ChatApp.Api.Domain.Exceptions
{
    public class RoomNotFoundException : DomainException
    {
        public RoomNotFoundException(string roomCode) : base($"Room {roomCode} does not exist")
        {
        }
    }
}
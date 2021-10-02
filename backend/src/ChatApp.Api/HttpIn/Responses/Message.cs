namespace ChatApp.Api.HttpIn.Responses
{
    using System;

    public record Message(int Id, string Text, DateTime Timestamp, int RoomId, int UserId);
}
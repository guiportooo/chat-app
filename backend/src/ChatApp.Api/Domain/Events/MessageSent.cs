namespace ChatApp.Api.Domain.Events
{
    using System;
    using MediatR;

    public record MessageSent(string Text, DateTime Timestamp, string UserName, string RoomCode) : INotification;
}
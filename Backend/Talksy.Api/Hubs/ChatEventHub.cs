using Microsoft.AspNetCore.SignalR;
using Talksy.Api.Dtos.Chat;
using Talksy.Api.Dtos.Message;
using Talksy.Api.Models;

namespace Talksy.Api.Hubs;

public interface IChatEventHub
{
    Task ChatCreated(ChatDto dto);
    Task GotNewMessage(MessageDto dto);
}

public class ChatEventHub : Hub<IChatEventHub>;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(User.JwtUserIdClaimName)?.Value;
    }
}
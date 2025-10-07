using AutoMapper;
using Talksy.Api.Dtos.Chat;
using Talksy.Api.Models;

namespace Talksy.Api.Profiles;

public sealed class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<Chat, ChatDto>();
        CreateMap<ChatDto, Chat>();
    }
}
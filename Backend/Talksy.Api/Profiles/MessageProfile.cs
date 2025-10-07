using AutoMapper;
using Talksy.Api.Dtos.Message;
using Talksy.Api.Models;

namespace Talksy.Api.Profiles;

public sealed class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageDto>();
    }
}
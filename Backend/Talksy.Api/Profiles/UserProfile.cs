using AutoMapper;
using Talksy.Api.Dtos.User;
using Talksy.Api.Models;
using Talksy.Api.Services;

namespace Talksy.Api.Profiles;

public sealed class UserIdResolver : IValueResolver<UserRegisterDto, User, Guid>
{
    public Guid Resolve(UserRegisterDto source, User destination, Guid destMember, ResolutionContext context)
    {
        return Guid.CreateVersion7();
    }
}

public sealed class PasswordHashResolver : IValueResolver<UserRegisterDto, User, string>, IValueResolver<UserLoginDto, User, string>
{
    private readonly IPasswordHashService _passwordHashService;

    public PasswordHashResolver(IPasswordHashService passwordHashService)
    {
        _passwordHashService = passwordHashService;
    }

    public string Resolve(UserRegisterDto source, User destination, string destMember, ResolutionContext context)
    {
        return _passwordHashService.GetPasswordHash(source.Password);
    }

    public string Resolve(UserLoginDto source, User destination, string destMember, ResolutionContext context)
    {
        return _passwordHashService.GetPasswordHash(source.Password);
    }
}

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterDto, User>()
            .ForMember(x => x.Id, x => x.MapFrom<UserIdResolver>())
            .ForMember(x => x.PasswordHash, x => x.MapFrom<PasswordHashResolver>());

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}
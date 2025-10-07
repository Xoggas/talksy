using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Talksy.Api.Data;
using Talksy.Api.Dtos.User;
using Talksy.Api.Helpers;
using Talksy.Api.Models;

namespace Talksy.Api.Services;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result<IEnumerable<UserDto>>> GetUsersByUsernameAsync(string username);
    Task<Result<UserDto>> RegisterUserAsync(UserRegisterDto user);
    Task<Result<string>> AuthorizeUserAsync(UserLoginDto dto);
}

public sealed class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IMapper _mapper;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserService(AppDbContext dbContext, IPasswordHashService passwordHashService, IMapper mapper,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _passwordHashService = passwordHashService;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var userModel = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (userModel is null)
        {
            return Result.Failure<UserDto>("Invalid user id");
        }

        var userReadDto = _mapper.Map<UserDto>(userModel);

        return Result.Success(userReadDto);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersByUsernameAsync(string username)
    {
        var userModels = await _dbContext.Users
            .Where(u => EF.Functions.Like(u.Username, $"%{username}%"))
            .ToListAsync();

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(userModels);

        return Result.Success(userDtos);
    }

    public async Task<Result<UserDto>> RegisterUserAsync(UserRegisterDto dto)
    {
        var userModel = _mapper.Map<User>(dto);
        var userExists = await _dbContext.Users.AnyAsync(u => u.Username == userModel.Username);

        if (userExists)
        {
            return Result.Failure<UserDto>("User with this username already exists");
        }

        await _dbContext.Users.AddAsync(userModel);

        await _dbContext.SaveChangesAsync();

        var userReadDto = _mapper.Map<UserDto>(userModel);

        return Result.Success(userReadDto);
    }

    public async Task<Result<string>> AuthorizeUserAsync(UserLoginDto dto)
    {
        var userModel = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (userModel is null)
        {
            return Result.Failure<string>("Invalid username or password");
        }

        var isPasswordCorrect = _passwordHashService.IsPasswordCorrect(dto.Password, userModel.PasswordHash);

        if (isPasswordCorrect is false)
        {
            return Result.Failure<string>("Invalid username or password");
        }

        var token = _jwtTokenGenerator.GenerateToken(userModel);

        return Result.Success(token);
    }
}
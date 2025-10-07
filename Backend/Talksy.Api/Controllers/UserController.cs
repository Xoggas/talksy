using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Talksy.Api.Dtos.User;
using Talksy.Api.Services;

namespace Talksy.Api.Controllers;

/// <summary>
/// Управление пользователями (регистрация, авторизация, получение информации).
/// </summary>
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Получение данных о пользователе, который делает запрос на основе JWT.
    /// </summary>
    /// <returns>Возвращает пользователя на основе данных из JWT токена.</returns>
    [HttpGet("whoami")]
    [Authorize]
    [SwaggerOperation(Summary = "Получение пользователя из JWT",
        Description = "Возвращает пользователя по данным из JWT.")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> GetUserIdFromJwt()
    {
        var userIdRaw = User.FindFirst(Models.User.JwtUserIdClaimName)?.Value;
        
        if (userIdRaw is null)
        {
            return NotFound();
        }
        
        var userId = Guid.Parse(userIdRaw);
        var result = await _userService.GetUserByIdAsync(userId);

        if (result.IsFailure)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Получить пользователей по имени пользователя (username).
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <returns>Данные пользователей.</returns>
    [HttpGet("find/{username}")]
    [SwaggerOperation(Summary = "Получить пользователей по username",
        Description = "Возвращает данные пользователей по имени пользователя.")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<UserDto>>> FindUsersByUsername(string username, IValidator<UserFindDto> validator)
    {
        var userFindDto = new UserFindDto
        {
            Username = username
        };

        var validationResult = await validator.ValidateAsync(userFindDto);

        if (validationResult.IsValid is false)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _userService.GetUsersByUsernameAsync(username);

        return Ok(result.Value);
    }

    /// <summary>
    /// Зарегистрировать нового пользователя.
    /// </summary>
    /// <param name="dto">Данные для регистрации.</param>
    /// <returns>JWT токен.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Регистрация пользователя", Description = "Создает нового пользователя в системе.")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterUserAsync(UserRegisterDto dto)
    {
        var result = await _userService.RegisterUserAsync(dto);

        if (result.IsFailure)
        {
            return Conflict(result.Error);
        }

        var userLoginDto = new UserLoginDto
        {
            Username = dto.Username,
            Password = dto.Password
        };

        return await AuthorizeUserAsync(userLoginDto);
    }

    /// <summary>
    /// Авторизовать пользователя и получить JWT-токен.
    /// </summary>
    /// <param name="dto">Логин и пароль.</param>
    /// <returns>JWT токен.</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Авторизация пользователя",
        Description = "Возвращает JWT-токен при успешной авторизации.")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AuthorizeUserAsync(UserLoginDto dto)
    {
        var result = await _userService.AuthorizeUserAsync(dto);

        if (result.IsFailure)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            Token = result.Value
        });
    }
}
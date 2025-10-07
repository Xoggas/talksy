namespace Talksy.Api.Dtos.User;

/// <summary>
/// DTO для авторизации пользователя.
/// </summary>
public sealed class UserLoginDto
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}
namespace Talksy.Api.Dtos.User;

/// <summary>
/// DTO для регистрации нового пользователя.
/// </summary>
public sealed class UserRegisterDto
{
    /// <summary>
    /// Уникальный логин пользователя.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}
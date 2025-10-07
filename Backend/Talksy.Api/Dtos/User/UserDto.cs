namespace Talksy.Api.Dtos.User;

/// <summary>
/// DTO для пользователя.
/// </summary>
public sealed class UserDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Уникальный логин пользователя.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
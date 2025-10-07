namespace Talksy.Api.Dtos.User;

/// <summary>
/// DTO для поиска пользователя.
/// </summary>
public class UserFindDto
{
    /// <summary>
    /// Логин пользователя для поиска (username).
    /// </summary>
    public string Username { get; set; } = string.Empty;
}
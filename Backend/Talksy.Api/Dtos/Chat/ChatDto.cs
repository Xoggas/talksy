using Talksy.Api.Dtos.User;

namespace Talksy.Api.Dtos.Chat;

/// <summary>
/// DTO для чата.
/// </summary>
public class ChatDto
{
    /// <summary>
    /// Уникальный идентификатор чата.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Первый участник чата.
    /// </summary>
    public UserDto ChatMember1 { get; init; } = null!;

    /// <summary>
    /// Второй участник чата.
    /// </summary>
    public UserDto ChatMember2 { get; init; } = null!;
}
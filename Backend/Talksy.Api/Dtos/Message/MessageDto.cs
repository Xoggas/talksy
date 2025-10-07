using Talksy.Api.Dtos.Chat;
using Talksy.Api.Dtos.User;

namespace Talksy.Api.Dtos.Message;

/// <summary>
/// DTO для сообщения.
/// </summary>
public sealed class MessageDto
{
    /// <summary>
    /// Уникальный идентификатор сообщения.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Время, когда сообщение было отправлено.
    /// </summary>
    public DateTime SentAt { get; init; }
    
    /// <summary>
    /// Чат, к которому относится сообщение.
    /// </summary>
    public ChatDto Chat { get; init; } = null!;

    /// <summary>
    /// Пользователь, который отправил сообщение.
    /// </summary>
    public UserDto Sender { get; init; } = null!;

    /// <summary>
    /// Содержимое (текст) сообщения.
    /// </summary>
    public string Content { get; init; } = string.Empty;
}
namespace Talksy.Api.Dtos.Message;

/// <summary>
/// DTO для создания сообщения.
/// </summary>
public class MessageCreateDto
{
    /// <summary>
    /// Уникальный идентификатор чата, в который отправляется сообщение.
    /// </summary>
    public Guid ChatId { get; init; }

    /// <summary>
    /// Уникальный идентификатор отправителя сообщения.
    /// </summary>
    public Guid SenderId { get; init; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public string Content { get; init; } = string.Empty;
}
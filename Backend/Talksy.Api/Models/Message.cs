using System.ComponentModel.DataAnnotations;

namespace Talksy.Api.Models;

public sealed class Message
{
    [Key]
    public Guid Id { get; init; }

    public DateTime SentAt { get; init; }
    public Chat Chat { get; init; } = null!;
    public User Sender { get; init; } = null!;

    [MaxLength(200)]
    public string Content { get; init; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace Talksy.Api.Models;

public sealed class Chat
{
    [Key]
    public Guid Id { get; init; }

    public User ChatMember1 { get; init; } = null!;
    public User ChatMember2 { get; init; } = null!;
}
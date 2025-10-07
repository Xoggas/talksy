using System.ComponentModel.DataAnnotations;

namespace Talksy.Api.Models;

public sealed class User
{
    public static string JwtUserIdClaimName = "user_id";

    [Key]
    public Guid Id { get; init; }

    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(60)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
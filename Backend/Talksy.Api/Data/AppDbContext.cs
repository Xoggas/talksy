using Microsoft.EntityFrameworkCore;
using Talksy.Api.Models;

namespace Talksy.Api.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
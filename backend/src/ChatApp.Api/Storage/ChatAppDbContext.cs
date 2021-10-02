namespace ChatApp.Api.Storage
{
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class ChatAppDbContext : DbContext
    {
        public const string DatabaseName = "ChatApp";

        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(typeof(ChatAppDbContext).Assembly);
    }
}
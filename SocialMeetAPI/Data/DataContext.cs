using Microsoft.EntityFrameworkCore;
using SocialMeetAPI.Models;

namespace SocialMeetAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base (options) {}
        public DbSet<Value> Values { get; set; } // set table thru entity framework
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

          builder
            .Entity<Like>()
            .HasKey(k => new { k.LikerId, k.LikeeId });

          builder
            .Entity<Like>()
            .HasOne(u => u.Likee)
            .WithMany(u => u.Likers)
            .HasForeignKey(u => u.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

          builder
            .Entity<Like>()
            .HasOne(u => u.Liker)
            .WithMany(u => u.Likees)
            .HasForeignKey(u => u.LikerId)
            .OnDelete(DeleteBehavior.Restrict);

          builder
            .Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessageSent)
            .OnDelete(DeleteBehavior.Restrict);
          
          builder
            .Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessageRecieved)
            .OnDelete(DeleteBehavior.Restrict);
          
        }

    }
}
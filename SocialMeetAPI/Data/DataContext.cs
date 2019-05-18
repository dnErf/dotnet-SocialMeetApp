using Microsoft.EntityFrameworkCore;
using SocialMeetAPI.Models;

namespace SocialMeetAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}
        public DbSet<Value> Values { get; set; }
    }
}
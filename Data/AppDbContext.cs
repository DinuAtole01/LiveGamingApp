using Microsoft.EntityFrameworkCore;
using LiveGamingApp.Models;

namespace LiveGamingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MatchRecord> MatchRecords { get; set; } // <-- ही नवीन लाईन ॲड केली
    }
}
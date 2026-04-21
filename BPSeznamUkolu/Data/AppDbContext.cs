using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;


namespace BPSeznamUkolu.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cheklist.db");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }
    }
}

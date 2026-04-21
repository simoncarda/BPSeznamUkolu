using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;


namespace BPSeznamUkolu.Data
{
    internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ChecklistItem> ChecklistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cheklist.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}

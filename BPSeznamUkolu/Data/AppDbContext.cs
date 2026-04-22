using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;


namespace BPSeznamUkolu.Data
{
    internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();
    }
}

using BPSeznamUkolu.Data;
using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;

namespace BPSeznamUkolu.Services
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext _context;
        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddChecklistItemAsync(ChecklistItem item)
        {
            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChecklistItemAsync(ChecklistItem item)
        {
            _context.ChecklistItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateChecklistItemAsync(ChecklistItem item)
        {
            _context.ChecklistItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChecklistItem>> GetChecklistItemsAsync()
        {
            return await _context.ChecklistItems.ToListAsync();
        }
    }
}

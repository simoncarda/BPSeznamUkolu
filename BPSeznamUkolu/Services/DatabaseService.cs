using BPSeznamUkolu.Data;
using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;

namespace BPSeznamUkolu.Services
{
    internal class DatabaseService(AppDbContext context) : IDatabaseService
    {
        public async Task AddChecklistItemAsync(ChecklistItem item)
        {
            context.ChecklistItems.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task DeleteChecklistItemAsync(ChecklistItem item)
        {
            context.ChecklistItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task UpdateChecklistItemAsync(ChecklistItem item)
        {
            context.ChecklistItems.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task<List<ChecklistItem>> GetChecklistItemsAsync()
        {
            return await context.ChecklistItems.ToListAsync();
        }
    }
}

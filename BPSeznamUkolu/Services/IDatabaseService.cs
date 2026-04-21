using BPSeznamUkolu.Models;

namespace BPSeznamUkolu.Services
{
    internal interface IDatabaseService
    {
        Task<List<ChecklistItem>> GetChecklistItemsAsync();
        Task AddChecklistItemAsync(ChecklistItem item);
        Task DeleteChecklistItemAsync(ChecklistItem item);
        Task UpdateChecklistItemAsync(ChecklistItem item);
    }
}

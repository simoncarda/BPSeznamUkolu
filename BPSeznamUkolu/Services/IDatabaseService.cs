using BPSeznamUkolu.Models;

namespace BPSeznamUkolu.Services
{
    internal interface IDatabaseService
    {
        event Action? OnDatabaseChanged;
        Task<List<ChecklistItem>> GetChecklistItemsAsync();
        Task AddChecklistItemAsync(ChecklistItem item);
        Task DeleteChecklistItemAsync(ChecklistItem item);
        Task UpdateChecklistItemAsync(ChecklistItem item);
    }
}

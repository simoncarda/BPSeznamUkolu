using BPSeznamUkolu.Models;
using SQLite;

namespace BPSeznamUkolu.Services
{
    internal class DatabaseService : IDatabaseService
    {
        public event Action? OnDatabaseChanged;

        private SQLiteAsyncConnection? _database;
        private const string DbName = "Checklist.db";

        // Synchronizace inicializace
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);
        private bool _initialized;

        private async Task InitAsync()
        {
            if (_initialized) {
                return;
            }

            await _initSemaphore.WaitAsync();
            try {
                // Znovu zkontrolujeme, zda již není inicializace provedena, protože
                // mezi čekáním na zámek a získáním zámku mohlo jiné vlákno inicializaci dokončit.
                if (_initialized) {
                    return;
                }

                string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
                _database = new SQLiteAsyncConnection(dbPath);
                await _database.CreateTableAsync<ChecklistItem>();

                OnDatabaseChanged?.Invoke();

                _initialized = true;
            }
            finally {
                _initSemaphore.Release();
            }
        }

        public async Task AddChecklistItemAsync(ChecklistItem item)
        {
            await InitAsync();

            await _database!.InsertAsync(item);
            OnDatabaseChanged?.Invoke();
        }

        public async Task DeleteChecklistItemAsync(ChecklistItem item)
        {
            await InitAsync();
            await _database!.DeleteAsync(item);
            OnDatabaseChanged?.Invoke();
        }

        public async Task UpdateChecklistItemAsync(ChecklistItem item)
        {
            await InitAsync();
            await _database!.UpdateAsync(item);
            OnDatabaseChanged?.Invoke();
        }

        public async Task<List<ChecklistItem>> GetChecklistItemsAsync()
        {
            await InitAsync();
            return await _database!.Table<ChecklistItem>().ToListAsync();
        }
    }
}

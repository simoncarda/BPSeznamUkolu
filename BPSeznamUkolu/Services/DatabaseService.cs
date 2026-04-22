using BPSeznamUkolu.Data;
using BPSeznamUkolu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BPSeznamUkolu.Services
{
    internal class DatabaseService(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<DatabaseService> logger) : IDatabaseService
    {
        public async Task AddChecklistItemAsync(ChecklistItem item)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            try {
                context.ChecklistItems.Add(item);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Chyba při přidávání položky {Name}", item.Name);
                throw new InvalidOperationException("Nepodařilo se uložit novou položku do databáze.", ex);
            }
        }

        public async Task DeleteChecklistItemAsync(ChecklistItem item)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();

            try {
                context.ChecklistItems.Remove(item);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Chyba při mazání položky s ID {Id}", item.Id);
                throw new InvalidOperationException("Nepodařilo se odstranit položku z databáze.", ex);
            }
        }

        public async Task UpdateChecklistItemAsync(ChecklistItem item)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();

            try {
                context.ChecklistItems.Update(item);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await context.Entry(item).ReloadAsync();
                logger.LogError(ex, "Chyba při aktualizaci položky {Id}", item.Id);
                throw new InvalidOperationException("Nepodařilo se aktualizovat data v databázi.", ex);
            }
        }

        public async Task<List<ChecklistItem>> GetChecklistItemsAsync()
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();

            try {
                return await context.ChecklistItems
                    .AsNoTracking()
                    .OrderBy(i => i.Id)
                    .ToListAsync();
            }
            catch (Exception ex) {
                logger.LogError(ex, "Chyba při načítání seznamu položek.");
                throw new InvalidOperationException("Nepodařilo se načíst data z databáze.", ex);
            }
        }
    }
}

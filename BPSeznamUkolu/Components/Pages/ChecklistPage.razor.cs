using BPSeznamUkolu.Models;
using BPSeznamUkolu.Services;
using Microsoft.AspNetCore.Components;

namespace BPSeznamUkolu.Components.Pages
{
    public partial class ChecklistPage : IDisposable
    {
        private string _newItemName = string.Empty;
        private string _newItemDescription = string.Empty;
        private List<ChecklistItem> _checklistItems = new List<ChecklistItem>();
        private string _errorMessage = string.Empty;

        [Inject]
        private IDatabaseService DatabaseService { get; set; } = default!;

        // Metoda pro přidání nové položky do checklistu
        // Validuje vstupní data a poté deleguje úkol na službu pro práci s databází
        private async Task OnAddChecklistItem()
        {
            if (String.IsNullOrWhiteSpace(_newItemName)) {
                _errorMessage = "Název položky nesmí být prázdný.";
                return;
            }

            var newItem = new ChecklistItem {
                Name = _newItemName,
                Description = _newItemDescription
            };

            await DatabaseService.AddChecklistItemAsync(newItem);
            _newItemName = string.Empty;
            _newItemDescription = string.Empty;
            _errorMessage = string.Empty;
        }

        // Metoda pro smazání položky z checklistu, pouze deleguje úkol na službu pro práci s databází
        private async Task OnDeleteChecklistItem(ChecklistItem item)
        {
            await DatabaseService.DeleteChecklistItemAsync(item);
        }

        // Metoda pro aktualizaci položky v checklistu, pouze deleguje úkol na službu pro práci s databází
        private async Task OnUpdateChecklistItem(ChecklistItem item)
        {
            await DatabaseService.UpdateChecklistItemAsync(item);
        }

        // Při inicializaci komponenty načítáme data z databáze a
        // přihlašujeme stránku k události změny databáze
        protected override async Task OnInitializedAsync()
        {
            _checklistItems = await DatabaseService.GetChecklistItemsAsync();
            DatabaseService.OnDatabaseChanged += OnDatabaseChanged;
        }

        // Metoda pro zpracování změn v databázi, načítá aktualizovaná data a aktualizuje zobrazení
        private async void OnDatabaseChanged()
        {
            _checklistItems = await DatabaseService.GetChecklistItemsAsync();
            StateHasChanged();
        }

        // Při zničení komponenty odhlašujeme stránku od události změny databáze
        public void Dispose()
        {
            DatabaseService.OnDatabaseChanged -= OnDatabaseChanged;
        }
    }
}

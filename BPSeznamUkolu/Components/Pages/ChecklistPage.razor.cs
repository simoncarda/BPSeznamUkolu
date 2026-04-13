using BPSeznamUkolu.Models;
using BPSeznamUkolu.Services;
using Microsoft.AspNetCore.Components;
using BPSeznamUkolu.Configuration;

namespace BPSeznamUkolu.Components.Pages
{
    public partial class ChecklistPage : IDisposable
    {
        private ChecklistItem _newItem = new ChecklistItem {
            Name = string.Empty,
            Description = string.Empty
        };
        private List<ChecklistItem> _checklistItems = new List<ChecklistItem>();
        private string _errorMessage = string.Empty;

        [Inject]
        private IDatabaseService DatabaseService { get; set; } = default!;

        // Metoda pro přidání nové položky do checklistu
        // Validuje vstupní data a poté deleguje úkol na službu pro práci s databází
        private async Task OnAddChecklistItem()
        {
            if (!IsItemValid(_newItem))
                return;
            try {
                await DatabaseService.AddChecklistItemAsync(_newItem);
            }
            catch (Exception ex) {
                _errorMessage = $"Chyba při přidávání položky: {ex.Message}";
                return;
            }
            _newItem.Name = string.Empty;
            _newItem.Description = string.Empty;
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
            if (!IsItemValid(item))
                return;
            try {
                await DatabaseService.UpdateChecklistItemAsync(item);
            }
            catch (Exception ex) {
                _errorMessage = $"Chyba při aktualizaci položky: {ex.Message}";
                return;
            }
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
            await InvokeAsync(StateHasChanged);
        }

        private bool IsItemValid(ChecklistItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name)) {
                _errorMessage = "Název položky nesmí být prázdný.";
                return false;
            }
            if (item.Name.Length > ChecklistSettings.MaxItemNameLength) {
                _errorMessage = $"Název položky nesmí být delší než {ChecklistSettings.MaxItemNameLength} znaků.";
                return false;
            }
            if (item.Description.Length > ChecklistSettings.MaxDescriptionLength) {
                _errorMessage = $"Popis položky nesmí být delší než {ChecklistSettings.MaxDescriptionLength} znaků.";
                return false;
            }
            _errorMessage = string.Empty;
            return true;
        }

        // Při zničení komponenty odhlašujeme stránku od události změny databáze
        public void Dispose()
        {
            DatabaseService.OnDatabaseChanged -= OnDatabaseChanged;
        }
    }
}

using BPSeznamUkolu.Models;
using BPSeznamUkolu.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace BPSeznamUkolu.Components.Pages
{
    public partial class ChecklistPage
    {
        private List<ChecklistItem> _checklistItems = new();
        private ChecklistItem _newItem = new();
        private string? _errorMessage = null;
        [Inject]
        private IDatabaseService DbService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try {
                _errorMessage = null;
                _checklistItems = await DbService.GetChecklistItemsAsync();
            }
            catch (Exception ex) {
                _errorMessage = ex.Message;
            }
        }

        private async Task OnAddChecklistItem()
        {
            try {
                await DbService.AddChecklistItemAsync(_newItem);
                _newItem = new ChecklistItem();
                await LoadDataAsync();
            }
            catch (Exception ex) {
                _errorMessage = ex.Message;
            }
        }

        private async Task OnUpdateChecklistItem(ChecklistItem item)
        {
            if (!IsItemValid(item))
                return;

            try {
                await DbService.UpdateChecklistItemAsync(item);
            }
            catch (Exception ex) {
                await LoadDataAsync();
                _errorMessage = ex.Message;
            }
        }
        private async Task OnDeleteChecklistItem(ChecklistItem item)
        {
            try {
                await DbService.DeleteChecklistItemAsync(item);
                await LoadDataAsync();
            }
            catch (Exception ex) {
                _errorMessage = ex.Message;
            }
        }
        private static bool IsItemValid(ChecklistItem item)
        {
            return string.IsNullOrEmpty(GetErrorMessage(item));
        }

        private static string GetErrorMessage(ChecklistItem item)
        {
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(item, context, results, true)) {
                return results.FirstOrDefault()?.ErrorMessage ?? "Chyba při validaci objektu";
            }
            return string.Empty;
        }
    }
}

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
        [Inject]
        private IDatabaseService _dbService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _checklistItems = await _dbService.GetChecklistItemsAsync();
        }

        private async Task OnAddChecklistItem()
        {
            await _dbService.AddChecklistItemAsync(_newItem);

            _newItem = new ChecklistItem();
            await LoadDataAsync();
        }
        private bool IsItemValid(ChecklistItem item)
        {
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(item, context, results, true);
        }

        private string GetErrorMessage(ChecklistItem item)
        {
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(item, context, results, true)) {
                return results.FirstOrDefault()?.ErrorMessage ?? "Chyba";
            }
            return string.Empty;
        }

        private async Task OnUpdateChecklistItem(ChecklistItem item)
        {
            if (!IsItemValid(item))
                return;

            await _dbService.UpdateChecklistItemAsync(item);
        }
        private async Task OnDeleteChecklistItem(ChecklistItem item)
        {
            await _dbService.DeleteChecklistItemAsync(item);
            await LoadDataAsync();
        }
    }
}

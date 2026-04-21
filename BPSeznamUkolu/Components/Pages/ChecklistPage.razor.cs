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
        private IDatabaseService DbService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _checklistItems = await DbService.GetChecklistItemsAsync();
        }

        private async Task OnAddChecklistItem()
        {
            await DbService.AddChecklistItemAsync(_newItem);

            _newItem = new ChecklistItem();
            await LoadDataAsync();
        }
        private static bool IsItemValid(ChecklistItem item)
        {
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(item, context, results, true);
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

        private async Task OnUpdateChecklistItem(ChecklistItem item)
        {
            if (!IsItemValid(item))
                return;

            await DbService.UpdateChecklistItemAsync(item);
        }
        private async Task OnDeleteChecklistItem(ChecklistItem item)
        {
            await DbService.DeleteChecklistItemAsync(item);
            await LoadDataAsync();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BPSeznamUkolu.Configuration;

namespace BPSeznamUkolu.Models
{
    [Table("ChecklistItems")]
    internal class ChecklistItem
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Název úkolu nesmí být prázdný.")]
        [MaxLength(ChecklistSettings.MaxItemNameLength, ErrorMessage = "Název je příliš dlouhý.")]
        public string Name { get; set;  } = string.Empty;

        [MaxLength(ChecklistSettings.MaxDescriptionLength, ErrorMessage = "Popis je příliš dlouhý.")]
        public string Description { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }
}

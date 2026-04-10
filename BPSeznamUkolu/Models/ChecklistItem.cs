using SQLite;
using BPSeznamUkolu.Configuration;

namespace BPSeznamUkolu.Models
{
    [Table("ChecklistItems")]
    internal class ChecklistItem
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        private string _name = string.Empty;
        private string _description = string.Empty;
        public bool IsChecked { get; set; } = false;

        // Tímto způsobem zajišťujeme, že ItemName a Description nikdy nebudou null
        // a zároveň budou dodržovat maximální délku definovanou v ChecklistSettings.
        [MaxLength(ChecklistSettings.MaxItemNameLength), NotNull]
        public string Name { 
            get { return _name; } 
            set {
                ArgumentNullException.ThrowIfNull(value);
                if(value.Length > ChecklistSettings.MaxItemNameLength)
                    throw new ArgumentException($"ItemName cannot be longer than {ChecklistSettings.MaxItemNameLength} characters.");
                _name = value; 
            } 
        }

        [MaxLength(ChecklistSettings.MaxDescriptionLength)]
        public string Description { 
            get { return _description; } 
            set {
                if(value.Length > ChecklistSettings.MaxDescriptionLength)
                    throw new ArgumentException($"Description cannot be longer than {ChecklistSettings.MaxDescriptionLength} characters.");
                _description = value; 
            } 
        }
    }
}

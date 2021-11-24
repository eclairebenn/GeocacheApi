using System.ComponentModel.DataAnnotations;

namespace GeocacheAPI.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z0-9 ]*$")]
        public string? Name { get; set; }

        public int GeocacheId { get; set; }
        public DateTime Activated { get; set; }
        public DateTime Deactivated { get; set; }

    }
}

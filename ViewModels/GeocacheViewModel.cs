using System.ComponentModel.DataAnnotations;

namespace GeocacheAPI.ViewModels
{
    public class GeocacheViewModel
    {
        public int ID { get; set; }

        [Required]

        public string? Moniker { get; set; }

        [Required]
        [MinLength(4)]
        public string? Name { get; set; }

        [MaxLength(3, ErrorMessage = "No more than 3 Items may be added to a Geocache object.")]
        public ICollection<ItemViewModel>? Items { get; set; }
        
        [Required]
        public LocationViewModel? Location { get; set; }


    }
}

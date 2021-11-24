using System.ComponentModel.DataAnnotations;
namespace GeocacheAPI.ViewModels
{
    public class LocationViewModel
    {
        public int Id { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}

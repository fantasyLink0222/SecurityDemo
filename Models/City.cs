using System.ComponentModel.DataAnnotations;

namespace SecurityDemo.Models
{
    public class City
    {
        [Required]
        [StringLength(100, ErrorMessage = "Input is too long.")]
        [RegularExpression(@"^[^;--selectinsertdropupdatedelete]*$", ErrorMessage = "Invalid input.")]
        public int cityId { get; set; }  // Primary key!
        public string cityName { get; set; }
        public List<Building> buildings { get; set; }

    }
}

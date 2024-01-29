using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace SecurityDemo.Models
{

    [Keyless]
    public class Product
    {
        [Required]
        [StringLength(100, ErrorMessage = "Input is too long.")]
        [RegularExpression(@"^[^;--selectinsertdropupdatedelete]*$", ErrorMessage = "Invalid input.")]
        public string prodID { get; set; }
        public string prodName { get; set; }
        public decimal price { get; set; }
    }
}

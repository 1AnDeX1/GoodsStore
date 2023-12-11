using GoodsStore.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        public string? Password { get; set; }
        public AccessTypes Access { get; set; }
        public ICollection<Orders>? Orders { get; set; }

    }
}

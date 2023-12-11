using GoodsStore.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public AccessTypes Access { get; set; }
    }
}

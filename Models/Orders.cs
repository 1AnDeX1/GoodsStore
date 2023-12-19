using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsStore.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public string AppUserID { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public AppUser AppUsers { get; set; }
        public ICollection<OrderItems>? OrderItems { get; set; }
    }
}

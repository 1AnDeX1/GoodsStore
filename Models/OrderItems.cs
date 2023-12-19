using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsStore.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemsID { get; set; }
        [ForeignKey(nameof(Orders))]
        public int OrderID { get; set; }
        [ForeignKey(nameof(Products))]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public Orders Order { get; set; }
        public Products Product { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string? Image {  get; set; }
        public ICollection<OrderItems>? OrderItems { get; set; }
        public ICollection<DeliveryQueue>? DeliveryQueues { get; set; }
    }
}
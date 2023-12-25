using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsStore.Models
{
    public class DeliveryQueue
    {
        [Key]
        public int DeliveryQueueID { get; set; }
        [ForeignKey(nameof(Products))]
        public int ProductID { get; set; }
        [ForeignKey(nameof(Orders))]
        public int OrderID { get; set; }
        public int QuantityRequest { get; set; }
        public DateTime Date {  get; set; }
        public Products? Product { get; set; }
        public Orders? Order { get; set; }
    }
}

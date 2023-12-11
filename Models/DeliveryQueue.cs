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
        public int QuantityRequest { get; set; }
        public DateTime Date {  get; set; }
        public Products? Product { get; set; }
    }
}

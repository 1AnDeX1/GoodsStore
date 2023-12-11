using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsStore.Models
{
    public class Orders
    {
        public int OrderID { get; set; }
        [ForeignKey("Customers")]
        public int CustomerID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        Customers Customer { get; set; }

    }
}

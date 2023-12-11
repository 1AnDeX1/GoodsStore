using GoodsStore.Data.Enum;

namespace GoodsStore.Models
{
    public class Customers
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public AccessTypes Access { get; set; }
        public ICollection<Orders> Orders{ get; set; }

    }
}

using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IOrderRepository
    {
        List<OrderItems> GetOrdersByUser(AppUser appUser);
    }
}

using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IOrderItemsRepository
    {
        IEnumerable<OrderItems> GetAllOrderItems();
        OrderItems GetByOrderID(int id);
        OrderItems GetByIdNoTracking(int id);
        OrderItems GetByProductId(int id);
        List<OrderItems> GetOrdersByUser(AppUser appUser);
        bool Add(OrderItems orderItems);
        bool Delete(OrderItems orderItem);
        bool Save();
    }
}

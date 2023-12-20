using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IOrderItemsRepository
    {
        IEnumerable<OrderItems> GetAllOrderItems();
        OrderItems GetByOrderId(int id);
        OrderItems GetByIdNoTracking(int id);
        OrderItems GetByProductId(int id);
        List<OrderItems> GetOrdersByUser(AppUser appUser);
        bool Add(OrderItems orderItems);
        //bool Buy(OrderItems orderItems);
        //bool Exist(OrderItems orderItems);
        //bool AddProductToDeliveryQueue();
        //bool Update(OrderItems orderItems);
        //bool Delete(OrderItems orderItems);
        bool Save();
    }
}

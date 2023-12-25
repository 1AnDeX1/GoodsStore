using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IOrderRepository
    {
        Orders GetById(int id);
        int GetLastOrderId();
        bool Add(Orders order);
        bool Update(Orders order);
        bool Delete(Orders order);
        bool Save();
    }
}

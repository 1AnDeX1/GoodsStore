using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IOrderRepository
    {
        Orders GetByProductId(int id);
        bool Update(Orders order);
        bool Save();
    }
}

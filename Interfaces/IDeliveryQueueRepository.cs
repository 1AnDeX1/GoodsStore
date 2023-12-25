using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IDeliveryQueueRepository
    {
        List<DeliveryQueue> GetAllDeliveryQueues();
        List<DeliveryQueue> GetAllByProductId(int id);
        DeliveryQueue GetByOrderId(int id);
        bool Add(DeliveryQueue deliveryQueue);
        bool Delete(DeliveryQueue deliveryQueue);
        bool Update(DeliveryQueue deliveryQueue);
        bool Save();
    }
}

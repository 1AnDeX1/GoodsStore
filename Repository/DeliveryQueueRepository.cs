using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Repository
{
    public class DeliveryQueueRepository : IDeliveryQueueRepository
    {
        private readonly AppDbContext _context;

        public DeliveryQueueRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<DeliveryQueue> GetAllDeliveryQueues()
        {
            return _context.DeliveryQueues.ToList();
        }
        public List<DeliveryQueue> GetAllByProductId(int id)
        {
            return _context.DeliveryQueues.Where(p => p.ProductID == id).ToList();
        }
        public bool Add(DeliveryQueue deliveryQueue)
        {
            _context.Add(deliveryQueue);
            return Save();
        }
        public bool Delete(DeliveryQueue deliveryQueue)
        {
            _context.Remove(deliveryQueue);
            return Save();
        }
        public bool Update(DeliveryQueue deliveryQueue)
        {
            _context.Update(deliveryQueue);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}

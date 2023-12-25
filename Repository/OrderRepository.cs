using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public Orders GetById(int id)
        {
            return _context.Orders.Find(id);
        }

        public int GetLastOrderId()
        {
            return _context.Orders.Max(order => order.OrderID);
        }
        public bool Add(Orders order)
        {
            _context.Add(order);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool Delete(Orders order)
        {
            _context.Remove(order);
            return Save();
        }
        public bool Update(Orders order)
        {
            _context.Update(order);
            return Save();
        }
    }
}

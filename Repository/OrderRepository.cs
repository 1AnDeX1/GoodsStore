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
        public List<OrderItems> GetOrdersByUser(AppUser appUser)
        {
            return _context.OrderItems.Include(p => p.Product).Include(o => o.Order).Where(u => u.Order.AppUserID == appUser.Id).ToList();
        }
    }
}

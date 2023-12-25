﻿using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Repository
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderItemsRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<OrderItems> GetAllOrderItems()
        {
            return _context.OrderItems.Include(p => p.Product).Include(o => o.Order).ToList();
        }

        public OrderItems GetByOrderID(int id)
        {
            return _context.OrderItems.FirstOrDefault(p => p.OrderID == id);
        }

        public OrderItems GetByProductId(int id)
        {
            return _context.OrderItems.Include(p => p.Product).Include(o => o.Order).FirstOrDefault(p => p.ProductID == id);
        }

        public OrderItems GetByIdNoTracking(int id)
        {
            return _context.OrderItems.AsNoTracking().FirstOrDefault(i => i.OrderItemsID == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Add(OrderItems orderItem)
        {
            _context.Add(orderItem);
            return Save();
        }
        public bool Delete(OrderItems orderItem)
        {
            _context.Remove(orderItem);
            return Save();
        }

        public List<OrderItems> GetOrdersByUser(AppUser appUser)
        {
            return _context.OrderItems.Include(p => p.Product).Include(o => o.Order).Where(u => u.Order.AppUserID == appUser.Id).ToList();
        }
    }
}

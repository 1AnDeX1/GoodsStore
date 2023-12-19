using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;

        public ProductsRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(Products product)
        {
            _context.Add(product);
            return Save();
        }

        public bool Delete(Products product)
        {
            _context.Remove(product);
            return Save();
        }

        public IEnumerable<Products> GetAll()
        {
            return _context.Products.ToList();
            
        }

        public Products GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductID == id);
        }
        public Products GetByIdNoTracking(int id)
        {
            return _context.Products.AsNoTracking().FirstOrDefault(i => i.ProductID == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Products product)
        {
            _context.Update(product);
            return Save();
        }
    }
}

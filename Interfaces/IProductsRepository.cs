using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products> GetByIdAsync(int id);
        Task<Products> GetByIdAsyncNoTracking(int id);
        bool Add(Products product);
        bool Update(Products product);
        bool Delete(Products product);
        bool Save();
    }
}

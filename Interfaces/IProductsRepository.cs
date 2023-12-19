using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<Products> GetAll();
        Products GetById(int id);
        Products GetByIdNoTracking(int id);
        bool Add(Products product);
        bool Update(Products product);
        bool Delete(Products product);
        bool Save();
    }
}

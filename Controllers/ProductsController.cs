using GoodsStore.Data;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            Products product = _context.Products.FirstOrDefault(p => p.ProductID == id); 
            return View(product);
        }
    }
}

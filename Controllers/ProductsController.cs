using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class OrderItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

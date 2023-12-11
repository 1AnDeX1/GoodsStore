using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

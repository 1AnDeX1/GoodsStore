using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class DeliveryQueueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

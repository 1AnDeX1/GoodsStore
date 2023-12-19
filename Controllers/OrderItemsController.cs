using AutoMapper;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using GoodsStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoodsStore.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(
            IOrderItemsRepository orderItemsRepository,
            IProductsRepository productsRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _orderItemsRepository = orderItemsRepository;
            _productsRepository = productsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public IActionResult AdminIndex()
        {
            var orderItems = _orderItemsRepository.GetAllOrderItems();
            return View(orderItems);
        }
        [HttpGet("OrderItems/Buy/{productid}")]
        public IActionResult Buy(int productId)
        {
            var product = _productsRepository.GetById(productId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost("OrderItems/Buy/{productid}")]
        public IActionResult Buy(int productId, int quantity)
        {
            var product = _productsRepository.GetById(productId);

            if (product == null)
                return NotFound(); 

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetUserById(userId);

            if (user == null)
            {
                return NotFound(); 
            }

            var order = new Orders
            {
                AppUserID = userId,
                Date = DateTime.Now,
                Status = "Not Done"
            };

            var orderItem = new OrderItems
            {
                ProductID = productId,
                Quantity = quantity,
                Order = order 
            };

            _orderItemsRepository.Add(orderItem);

            return RedirectToAction("Index", "Products");
        }

    }
}

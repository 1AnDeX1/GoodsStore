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
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(
            IOrderItemsRepository orderItemsRepository,
            IProductsRepository productsRepository,
            IUserRepository userRepository,
            IDeliveryQueueRepository deliveryQueueRepository,
            IMapper mapper)
        {
            _orderItemsRepository = orderItemsRepository;
            _productsRepository = productsRepository;
            _userRepository = userRepository;
            _deliveryQueueRepository = deliveryQueueRepository;
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

            if (product.Quantity >= quantity)
            {
                var order = new Orders
                {
                    AppUserID = userId,
                    Date = DateTime.Now,
                    Status = "Done" 
                };

                var orderItem = new OrderItems
                {
                    ProductID = productId,
                    Quantity = quantity,
                    Order = order
                };

                _orderItemsRepository.Add(orderItem);

                product.Quantity -= quantity;
                _productsRepository.Update(product);

                return RedirectToAction("Index", "Products");
            }
            else
            {
                var deliveryRequest = new DeliveryQueue
                {
                    ProductID = productId,
                    QuantityRequest = quantity,
                    Date = DateTime.Now
                };

                _deliveryQueueRepository.Add(deliveryRequest);

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
}

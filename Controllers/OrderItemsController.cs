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
        private readonly IOrderRepository _orderRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;
        private readonly IMapper _mapper;

        public OrderItemsController(
            IOrderItemsRepository orderItemsRepository,
            IOrderRepository orderRepository,
            IProductsRepository productsRepository,
            IUserRepository userRepository,
            IDeliveryQueueRepository deliveryQueueRepository,
            IMapper mapper)
        {
            _orderItemsRepository = orderItemsRepository;
            _orderRepository = orderRepository;
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

                _orderRepository.Add(order);

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
                var order = new Orders
                {
                    AppUserID = userId,
                    Date = DateTime.Now,
                    Status = "Not Done"
                };
                _orderRepository.Add(order);
                var lastOrderId = _orderRepository.GetLastOrderId();

                var orderItem = new OrderItems
                {
                    OrderID = lastOrderId,
                    ProductID = productId,
                    Quantity = quantity,
                    Order = order
                };

                

                var deliveryRequest = new DeliveryQueue
                {
                    ProductID = productId,
                    OrderID = lastOrderId,
                    QuantityRequest = quantity,
                    Date = DateTime.Now
                };

                _deliveryQueueRepository.Add(deliveryRequest);

                _orderItemsRepository.Add(orderItem);

                return RedirectToAction("Index", "Products");
            }
        }

        [HttpGet("OrderItems/Cancel/{orderid}")]
        public IActionResult Cancel(int orderid)
        {
            var orderItem = _orderItemsRepository.GetByOrderID(orderid);
            var order = _orderRepository.GetById(orderid);
            if (orderItem != null && order.Status == "Not Done")
            {
                order.Status = "Cancelled";
                _orderRepository.Update(order);

                var deliveryRequest = _deliveryQueueRepository.GetByOrderId(orderid);
                if (deliveryRequest != null)
                {
                    _deliveryQueueRepository.Delete(deliveryRequest);
                }

                return RedirectToAction("UserOrders", "Account");
            }

            return NotFound();
        }

    }
}

using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GoodsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsApiController : ControllerBase
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;

        public OrderItemsApiController(
            IOrderItemsRepository orderItemsRepository,
            IOrderRepository orderRepository,
            IProductsRepository productsRepository,
            IUserRepository userRepository,
            IDeliveryQueueRepository deliveryQueueRepository)
        {
            _orderItemsRepository = orderItemsRepository;
            _orderRepository = orderRepository;
            _productsRepository = productsRepository;
            _userRepository = userRepository;
            _deliveryQueueRepository = deliveryQueueRepository;
        }

        [HttpGet("admin")]
        public ActionResult<IEnumerable<OrderItems>> AdminIndex()
        {
            var orderItems = _orderItemsRepository.GetAllOrderItems();
            return Ok(orderItems);
        }

        [HttpGet("buy/{productId}")]
        public IActionResult Buy(int productId)
        {
            var product = _productsRepository.GetById(productId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("buy/{productId}")]
        public IActionResult Buy(int productId, int quantity)
        {
            var product = _productsRepository.GetById(productId);

            if (product == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetUserById(userId);

            if (user == null)
                return NotFound();

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

                return Ok("Order placed successfully");
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

                return Ok("Order placed in queue");
            }
        }

        [HttpGet("cancel/{orderId}")]
        public IActionResult Cancel(int orderId)
        {
            var orderItem = _orderItemsRepository.GetByOrderID(orderId);
            var order = _orderRepository.GetById(orderId);
            if (orderItem != null && order.Status == "Not Done")
            {
                order.Status = "Cancelled";
                _orderRepository.Update(order);

                var deliveryRequest = _deliveryQueueRepository.GetByOrderId(orderId);
                if (deliveryRequest != null)
                {
                    _deliveryQueueRepository.Delete(deliveryRequest);
                }

                return Ok("Order cancelled");
            }

            return NotFound();
        }
    }
}

using AutoMapper;
using GoodsStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class DeliveryQueueController : Controller
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;
        private readonly IMapper _mapper;

        public DeliveryQueueController(
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
        public IActionResult Index()
        {
            var deliveryQueues = _deliveryQueueRepository.GetAllDeliveryQueues();
            return View(deliveryQueues);
        }
    }
}

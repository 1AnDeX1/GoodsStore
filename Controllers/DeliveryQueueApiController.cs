using AutoMapper;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GoodsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryQueueApiController : ControllerBase
    {
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;

        public DeliveryQueueApiController(IDeliveryQueueRepository deliveryQueueRepository)
        {
            _deliveryQueueRepository = deliveryQueueRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeliveryQueue>> GetAllDeliveryQueues()
        {
            var deliveryQueues = _deliveryQueueRepository.GetAllDeliveryQueues();
            return Ok(deliveryQueues);
        }

        [HttpGet("order/{id}")]
        public IActionResult GetByOrderId(int id)
        {
            var deliveryQueue = _deliveryQueueRepository.GetByOrderId(id);
            if (deliveryQueue == null)
                return NotFound();

            return Ok(deliveryQueue);
        }

        [HttpGet("product/{id}")]
        public IActionResult GetAllByProductId(int id)
        {
            var deliveryQueues = _deliveryQueueRepository.GetAllByProductId(id);
            return Ok(deliveryQueues);
        }
    }
}

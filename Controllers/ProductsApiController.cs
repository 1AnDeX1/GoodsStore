using AutoMapper;
using GoodsStore.Data;
using GoodsStore.Dto;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GoodsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IDeliveryQueueRepository deliveryQueueRepository,
            IOrderItemsRepository orderItemsRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _deliveryQueueRepository = deliveryQueueRepository;
            _orderItemsRepository = orderItemsRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Products>))]
        public IActionResult GetAllProducts()
        {
            var products = _productsRepository.GetAll();
            var productDtos = _mapper.Map<List<ProductsDto>>(products);
            return Ok(productDtos);
        }


        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productsRepository.GetById(id);
            if (product == null)
            {
                return NotFound(); 
            }

            var productDto = _mapper.Map<ProductsDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductsDto productDto)
        {
            var product = _mapper.Map<Products>(productDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productsRepository.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, productDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductsDto productDto)
        {
            if (id != productDto.ProductID)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Products>(productDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productsRepository.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productsRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productsRepository.Delete(product);
            return NoContent();
        }
    }
}

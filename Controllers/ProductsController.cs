using AutoMapper;
using GoodsStore.Data;
using GoodsStore.Dto;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using GoodsStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace GoodsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IDeliveryQueueRepository _deliveryQueueRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ProductsController(
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
        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products);
        }
        #region Standart Logic
        public IActionResult Detail(int id)
        {
            var product = _productsRepository.GetById(id); 
            return View(product);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Products product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.Add(product);
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var product =  _productsRepository.GetById(id);
            if(product == null) 
                return View("Error");
            var newproduct = new Products
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
            };
            return View(newproduct);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductsDto productDto)
        {
            var product = _mapper.Map<ProductsDto>(productDto);
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                return View("Edit", product);
            }

            var userproduct = _productsRepository.GetByIdNoTracking(id);

            if (userproduct != null)
            {
                var editedproduct = new Products
                {
                    ProductID = id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Image = product.Image,
                };

                ProcessDeliveryQueue(editedproduct);

                _productsRepository.Update(editedproduct);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public IActionResult Delete(int id)
        {
            var productDetails = _productsRepository.GetById(id);
            if (productDetails == null)
                return View("Error");
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            var productDetails = _productsRepository.GetById(id);
            if (productDetails == null)
                return View("Error", 404);
            _productsRepository.Delete(productDetails);
            return RedirectToAction("Index");
        }
        #endregion

        public void ProcessDeliveryQueue(Products product)
        {
            var deliveryQueueItems = _deliveryQueueRepository.GetAllByProductId(product.ProductID);

            if (deliveryQueueItems != null && deliveryQueueItems.Any())
            {
                foreach (var deliveryItem in deliveryQueueItems.OrderBy(d => d.Date))
                {
                    if (product.Quantity >= deliveryItem.QuantityRequest)
                    {
                        var orderItem = _orderItemsRepository.GetByProductId(product.ProductID);
                        if (orderItem != null)
                        {
                            var order = orderItem.Order;
                            if (order != null)
                            {
                                order.Status = "Done";
                                _orderRepository.Update(order);
                            }
                        }

                        _deliveryQueueRepository.Delete(deliveryItem);
                        product.Quantity -= deliveryItem.QuantityRequest;
                    }
                }
            }
        }
    }
}

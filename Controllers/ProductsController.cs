using AutoMapper;
using GoodsStore.Dto;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create(ProductsDto productDto)
        {
            var product = _mapper.Map<Products>(productDto);

            if (!ModelState.IsValid)
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
            var product = _mapper.Map<Products>(productDto);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                return View("Edit", product);
            }

            var userproduct = _productsRepository.GetById(id);
            var quantity = product.Quantity;
            if (userproduct != null)
            {
                userproduct.Name = product.Name;
                userproduct.Description = product.Description;
                userproduct.Price = product.Price;
                userproduct.Quantity = product.Quantity;
                userproduct.Image = product.Image;

                _productsRepository.Update(userproduct);

                var deliveryRequests = _deliveryQueueRepository.GetAllByProductId(id);
                if (deliveryRequests != null)
                {
                    foreach (var request in deliveryRequests.OrderBy(d => d.Date))
                    {
                        if (userproduct.Quantity >= request.QuantityRequest)
                        {
                            userproduct.Quantity -= request.QuantityRequest;

                            _productsRepository.Update(userproduct);

                            _deliveryQueueRepository.Delete(request);

                            var order = _orderRepository.GetById(request.OrderID);
                            if (order != null)
                            {
                                order.Status = "Done";
                                _orderRepository.Update(order);
                            }
                        }
                    }
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                return View(productDto);
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
        [HttpGet]
        public IActionResult Add(int id)
        {
            var product = _productsRepository.GetById(id);

            if (product == null)
            {
                return NotFound(); // Обробити відсутній товар
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Add(int productId, int quantity)
        {
            var product = _productsRepository.GetById(productId);


            product.Quantity += quantity;

            _productsRepository.Update(product);

            var deliveryRequests = _deliveryQueueRepository.GetAllByProductId(productId);
            if (deliveryRequests != null)
            {
                foreach (var request in deliveryRequests.OrderBy(d => d.Date))
                {
                    if (product.Quantity >= request.QuantityRequest)
                    {
                        product.Quantity -= request.QuantityRequest;

                        _productsRepository.Update(product);

                        _deliveryQueueRepository.Delete(request);

                        var order = _orderRepository.GetById(request.OrderID);
                        if (order != null)
                        {
                            order.Status = "Done";
                            _orderRepository.Update(order);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}

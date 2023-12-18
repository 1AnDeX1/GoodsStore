using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace GoodsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository) 
        {
            _productsRepository = productsRepository;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productsRepository.GetAll();
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Products product = await _productsRepository.GetByIdAsync(id); 
            return View(product);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Products product)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.Add(product);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(int id, Products product)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                return View("Edit", product);
            }

            var userproduct = await _productsRepository.GetByIdAsyncNoTracking(id);
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
                _productsRepository.Update(editedproduct);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
    }
}

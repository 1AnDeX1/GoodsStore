using AutoMapper;
using GoodsStore.Data;
using GoodsStore.Dto;
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
        private readonly IMapper _mapper;

        public ProductsController(IProductsRepository productsRepository, IMapper mapper) 
        {
            _productsRepository = productsRepository;
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
        public IActionResult Edit(int id, Products product)
        {
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


    }
}

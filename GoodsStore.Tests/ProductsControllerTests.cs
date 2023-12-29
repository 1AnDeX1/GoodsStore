using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GoodsStore.Controllers;
using GoodsStore.Dto;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GoodsStore.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsRepository> _mockProductsRepo = new Mock<IProductsRepository>();
        private readonly Mock<IDeliveryQueueRepository> _mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();
        private readonly Mock<IOrderRepository> _mockOrderRepo = new Mock<IOrderRepository>();
        private readonly Mock<IOrderItemsRepository> _mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        [Fact]
        public void Index_ReturnsViewResult_WithListOfProducts()
        {
            // Arrange
            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                _mockOrderItemsRepo.Object,
                _mockOrderRepo.Object,
                _mockMapper.Object);

            var products = new List<Products>
            {
                new Products { ProductID = 1, Name = "Product 1" },
                new Products { ProductID = 2, Name = "Product 2" }
            };

            _mockProductsRepo.Setup(repo => repo.GetAll()).Returns(products);

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as List<Products>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Detail_ReturnsViewResult_WithProduct()
        {
            // Arrange
            var controller = new ProductsController(
                _mockProductsRepo.Object,
                null, // Other repositories not needed for this test
                null,
                null,
                _mockMapper.Object);

            var product = new Products { ProductID = 1, Name = "Sample Product" };

            _mockProductsRepo.Setup(repo => repo.GetById(1)).Returns(product);

            // Act
            var result = controller.Detail(1) as ViewResult;
            var model = result?.Model as Products;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal("Sample Product", model.Name);
            Assert.Equal(1, model.ProductID);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new ProductsController(
                null, // Not using repository for this test
                null,
                null,
                null,
                null); // Not using mapper for this test

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        

        [Fact]
        public void Create_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            var productDto = new ProductsDto { /* Fill in valid properties */ };

            // Act
            var result = controller.Create(productDto) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                _mockOrderItemsRepo.Object,
                _mockOrderRepo.Object,
                _mockMapper.Object);

            controller.ModelState.AddModelError("Name", "Name is required");

            var productDto = new ProductsDto(); // Invalid model

            // Act
            var result = controller.Create(productDto) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ViewName); // Check that it returns the default view
        }

        [Fact]
        public void Delete_ExistingId_ReturnsViewResultWithProduct()
        {
            // Arrange
            var product = new Products { ProductID = 1, Name = "Sample Product" };
            _mockProductsRepo.Setup(repo => repo.GetById(1)).Returns(product);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;
            var model = result?.Model as Products;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal("Sample Product", model.Name);
            Assert.Equal(1, model.ProductID);
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsErrorView()
        {
            // Arrange
            _mockProductsRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<Products>(null);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }

        [Fact]
        public void DeleteProduct_ExistingId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var product = new Products { ProductID = 1, Name = "Sample Product" };
            _mockProductsRepo.Setup(repo => repo.GetById(1)).Returns(product);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.DeleteProduct(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeleteProduct_NonExistingId_ReturnsErrorView()
        {
            // Arrange
            _mockProductsRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<Products>(null);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.DeleteProduct(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }

        [Fact]
        public void Add_GET_ExistingProductId_ReturnsViewResultWithProduct()
        {
            // Arrange
            var product = new Products { ProductID = 1, Name = "Sample Product" };
            _mockProductsRepo.Setup(repo => repo.GetById(1)).Returns(product);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.Add(1) as ViewResult;
            var model = result?.Model as Products;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal("Sample Product", model.Name);
            Assert.Equal(1, model.ProductID);
        }

        [Fact]
        public void Add_GET_NonExistingProductId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockProductsRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<Products>(null);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                null,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.Add(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Add_POST_ValidProduct_ReturnsRedirectToActionResult()
        {
            // Arrange
            var product = new Products { ProductID = 1, Name = "Sample Product" };
            _mockProductsRepo.Setup(repo => repo.GetById(1)).Returns(product);

            var controller = new ProductsController(
                _mockProductsRepo.Object,
                _mockDeliveryQueueRepo.Object,
                _mockOrderItemsRepo.Object,
                null,
                _mockMapper.Object);

            // Act
            var result = controller.Add(1, 5) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

    }
}

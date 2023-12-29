using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using GoodsStore.Controllers;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GoodsStore.Tests.Controllers
{
    public class OrderItemsControllerTests
    {
        private readonly Mock<IOrderItemsRepository> _mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
        private readonly Mock<IOrderRepository> _mockOrderRepo = new Mock<IOrderRepository>();
        private readonly Mock<IProductsRepository> _mockProductsRepo = new Mock<IProductsRepository>();
        private readonly Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
        private readonly Mock<IDeliveryQueueRepository> _mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();


        [Fact]
        public void Buy_POST_WithSufficientQuantity_RedirectsToProductsIndex()
        {
            // Arrange
            var productId = 1;
            var quantity = 5;
            var userId = "user_id";

            var product = new Products { ProductID = productId, Quantity = 10 /* Assuming available quantity */ };
            _mockProductsRepo.Setup(repo => repo.GetById(productId)).Returns(product);

            var user = new AppUser { Id = userId };
            _mockUserRepo.Setup(repo => repo.GetUserById(userId)).Returns(user);

            var order = new Orders();
            _mockOrderRepo.Setup(repo => repo.Add(It.IsAny<Orders>())).Returns(true);

            _mockOrderRepo.Setup(repo => repo.GetLastOrderId()).Returns(1);

            var controller = new OrderItemsController(
                _mockOrderItemsRepo.Object,
                _mockOrderRepo.Object,
                _mockProductsRepo.Object,
                _mockUserRepo.Object,
                _mockDeliveryQueueRepo.Object,
                _mockMapper.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) })) } };

            // Act
            var result = controller.Buy(productId, quantity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to Products/Index
            _mockOrderItemsRepo.Verify(repo => repo.Add(It.IsAny<OrderItems>()), Times.Once);
            _mockProductsRepo.Verify(repo => repo.Update(product), Times.Once);
            _mockDeliveryQueueRepo.Verify(repo => repo.Add(It.IsAny<DeliveryQueue>()), Times.Never);
        }

        [Fact]
        public void AdminIndex_ReturnsViewWithOrderItems()
        {
            // Arrange
            var orderItems = new List<OrderItems>
            {
                new OrderItems { OrderItemsID = 1, Quantity = 5 },
                new OrderItems { OrderItemsID = 2, Quantity = 3 }
            };
            var mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
            mockOrderItemsRepo.Setup(repo => repo.GetAllOrderItems()).Returns(orderItems);
            var controller = new OrderItemsController(mockOrderItemsRepo.Object, null, null, null, null, null);

            // Act
            var result = controller.AdminIndex() as ViewResult;
            var model = result?.Model as List<OrderItems>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(orderItems.Count, model?.Count()); // Check if the count of returned items matches the expected count
        }

        [Fact]
        public void Buy_ExistingProductId_ReturnsViewWithProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Products { ProductID = productId, Name = "Sample Product" };
            var mockProductsRepo = new Mock<IProductsRepository>();
            mockProductsRepo.Setup(repo => repo.GetById(productId)).Returns(product);
            var controller = new OrderItemsController(null, null, mockProductsRepo.Object, null, null, null);

            // Act
            var result = controller.Buy(productId) as ViewResult;
            var model = result?.Model as Products;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(product, model); // Check if the returned product matches the expected product
        }

        [Fact]
        public void Buy_NonExistingProductId_ReturnsNotFound()
        {
            // Arrange
            var productId = 1;
            var mockProductsRepo = new Mock<IProductsRepository>();
            mockProductsRepo.Setup(repo => repo.GetById(productId)).Returns<Products>(null);
            var controller = new OrderItemsController(null, null, mockProductsRepo.Object, null, null, null);

            // Act
            var result = controller.Buy(productId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void Buy_WithSufficientQuantity_RedirectsToProductsIndex()
        {
            // Arrange
            var productId = 1;
            var quantity = 5;
            var product = new Products { ProductID = productId, Quantity = 10 }; // Assuming initial quantity is 10
            var user = new AppUser { Id = "user_id" };
            var mockProductsRepo = new Mock<IProductsRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
            var mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();

            mockProductsRepo.Setup(repo => repo.GetById(productId)).Returns(product);
            mockUserRepo.Setup(repo => repo.GetUserById(It.IsAny<string>())).Returns(user);

            var controller = new OrderItemsController(
                mockOrderItemsRepo.Object,
                mockOrderRepo.Object,
                mockProductsRepo.Object,
                mockUserRepo.Object,
                mockDeliveryQueueRepo.Object,
                null // Assuming no mapper is used
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user_id")
                    }))
                }
            };

            // Act
            var result = controller.Buy(productId, quantity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to Index action after successful purchase
            Assert.Equal(5, product.Quantity); // Assuming 5 subtracted from initial quantity of 10

            mockProductsRepo.Verify(repo => repo.Update(product), Times.Once);
            mockOrderRepo.Verify(repo => repo.Add(It.IsAny<Orders>()), Times.Once);
            mockOrderItemsRepo.Verify(repo => repo.Add(It.IsAny<OrderItems>()), Times.Once);
            mockDeliveryQueueRepo.Verify(repo => repo.Add(It.IsAny<DeliveryQueue>()), Times.Never); // Assuming no delivery request added
        }

        [Fact]
        public void Buy_WithInsufficientQuantity_CreatesDeliveryRequestAndRedirectsToProductsIndex()
        {
            // Arrange
            var productId = 1;
            var quantity = 15; // More than available quantity (assuming available quantity is 10)
            var product = new Products { ProductID = productId, Quantity = 10 }; // Assuming initial quantity is 10
            var user = new AppUser { Id = "user_id" };
            var mockProductsRepo = new Mock<IProductsRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
            var mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();

            mockProductsRepo.Setup(repo => repo.GetById(productId)).Returns(product);
            mockUserRepo.Setup(repo => repo.GetUserById(It.IsAny<string>())).Returns(user);
            mockOrderRepo.Setup(repo => repo.GetLastOrderId()).Returns(1);

            var controller = new OrderItemsController(
                mockOrderItemsRepo.Object,
                mockOrderRepo.Object,
                mockProductsRepo.Object,
                mockUserRepo.Object,
                mockDeliveryQueueRepo.Object,
                null // Assuming no mapper is used
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user_id")
                    }))
                }
            };

            // Act
            var result = controller.Buy(productId, quantity) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to Index action after successful purchase

            mockProductsRepo.Verify(repo => repo.Update(product), Times.Never); // Assuming product quantity is not updated
            mockOrderRepo.Verify(repo => repo.Add(It.IsAny<Orders>()), Times.Once);
            mockOrderItemsRepo.Verify(repo => repo.Add(It.IsAny<OrderItems>()), Times.Once);
            mockDeliveryQueueRepo.Verify(repo => repo.Add(It.IsAny<DeliveryQueue>()), Times.Once); // Assuming delivery request added
        }
        [Fact]
        public void Cancel_WithNotDoneOrderAndOrderItem_ReturnsRedirectToAction()
        {
            // Arrange
            var orderId = 1;
            var orderItem = new OrderItems { OrderID = orderId };
            var order = new Orders { OrderID = orderId, Status = "Not Done" };

            var mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
            mockOrderItemsRepo.Setup(repo => repo.GetByOrderID(orderId)).Returns(orderItem);

            var mockOrderRepo = new Mock<IOrderRepository>();
            mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(order);

            var mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();

            var controller = new OrderItemsController(
                mockOrderItemsRepo.Object,
                mockOrderRepo.Object,
                null, // Assuming no other dependencies are needed
                null,
                mockDeliveryQueueRepo.Object,
                null
            );

            // Act
            var result = controller.Cancel(orderId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserOrders", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }

        

        [Fact]
        public void Cancel_WithNullOrderItem_ReturnsNotFound()
        {
            // Arrange
            var orderId = 1;

            var mockOrderItemsRepo = new Mock<IOrderItemsRepository>();
            mockOrderItemsRepo.Setup(repo => repo.GetByOrderID(orderId)).Returns((OrderItems)null);

            var mockOrderRepo = new Mock<IOrderRepository>();

            var mockDeliveryQueueRepo = new Mock<IDeliveryQueueRepository>();

            var controller = new OrderItemsController(
                mockOrderItemsRepo.Object,
                mockOrderRepo.Object,
                null,
                null,
                mockDeliveryQueueRepo.Object,
                null
            );

            // Act
            var result = controller.Cancel(orderId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

    }
}

using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using UnitTest.MVC.Web.Models;
using UnitTest.MVC.Web.Repository;

namespace xUnitTest.Test.UnitTest.MVC
{
    public class ProductEndpointsTest
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly IEnumerable<Product> _products;

        public ProductEndpointsTest()
        {
            this._mockRepository = new Mock<IRepository<Product>>();

            this._products = new List<Product>()
            {
                new Product { Id = 1, Name = "Pencil", Price = 10, Stock = 100, Color = "Red" },
                new Product { Id = 2, Name = "Phone", Price = 20, Stock = 200, Color = "Blue" },
                new Product { Id = 3, Name = "Mouse", Price = 30, Stock = 300, Color = "Green" },
                new Product { Id = 4, Name = "Keyboard", Price = 40, Stock = 400, Color = "Black"},
                new Product { Id = 5, Name = "Monitor", Price = 50, Stock = 500, Color = "White"},
                new Product { Id = 6, Name = "Laptop", Price = 60, Stock = 600, Color = "Silver"}
            };
        }

        [Fact]
        public async Task GetAll_ReturnsOk_AndProducts()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(_products.ToList());

            await using var factory = new TestAppFactory(_mockRepository.Object);
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.NotNull(result);
            Assert.Equal(_products.Count(), result!.Count);

            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }




    }
}

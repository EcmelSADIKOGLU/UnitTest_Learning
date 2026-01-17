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

        private (TestAppFactory factory, HttpClient client) CreateClient()
        {
            var factory = new TestAppFactory(_mockRepository.Object);
            var client = factory.CreateClient();
            return (factory, client);
        }


        [Fact]
        public async Task GetAll_SimpleCall_ReturnsOkWithProducts()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_products.ToList());

            var (factory, client) = CreateClient();

            await using (factory)
            {
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

        [Fact]
        public async Task GetById_NotExistProduct_ReturnNotFound()
        {
            // Arrange
            int id = 0;

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Product?)null);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.GetAsync($"/api/Products/{id}");

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                Assert.True(string.IsNullOrWhiteSpace(body));

                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            }


        }

        [Fact]
        public async Task GetById_ExistProduct_ReturnOkWithProduct()
        {
            // Arrange
            int id = 1;
            Product product = _products.First(x => x.Id == id);

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.GetAsync($"/api/Products/{id}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<Product?>();
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.Id);
                Assert.Equal(product.Name, result.Name);

                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            }

        }

        [Fact]
        public async Task Delete_NotExistProduct_ReturnNotFound()
        {
            // Arrange
            int id = 0;

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Product?)null);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.DeleteAsync($"/api/Products/{id}");

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                Assert.True(string.IsNullOrWhiteSpace(body));

                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
                _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
            }
        }

        [Fact]
        public async Task Delete_ExistProduct_ReturnOK()
        {
            // Arrange
            int id = 1;
            Product product = _products.First(x => x.Id == id);

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.DeleteAsync($"/api/Products/{id}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);


                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
                _mockRepository.Verify(r => r.DeleteAsync(It.Is<Product>(p => p.Id == id)), Times.Once);

            }
        }

        [Fact]
        public async Task Add_SimpleProduct_ReturnCreatedWithProductAndUrl()
        {
            // Arrange
            int id = 1;
            Product product = _products.First(x => x.Id == id);

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.PostAsJsonAsync("/api/Products", product);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal($"/api/Products/{product.Id}", response.Headers.Location?.ToString());

                var result = await response.Content.ReadFromJsonAsync<Product>();
                Assert.NotNull(result);
                Assert.Equal(product.Id, result!.Id);
                Assert.Equal(product.Name, result.Name);

                _mockRepository.Verify(r => r.AddAsync(It.Is<Product>(p => p.Id == id)), Times.Once);

            }
        }

        [Fact]
        public async Task Edit_NotExistProduct_ReturnNotFound()
        {
            // Arrange
            int id = 0;
            Product product = new Product { Id = id, Name = "Test", Price = 100, Stock = 10, Color = "Yellow" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Product?)null);
            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.PutAsJsonAsync("/api/Products", product);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                Assert.True(string.IsNullOrWhiteSpace(body));

                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
                _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
            }
        }

        [Fact]
        public async Task Edit_ExistProduct_ReturnOK()
        {
            //Arrange
            int id = 1;
            Product existingProduct = _products.First(x => x.Id == id);
            Product updatedProduct = new Product { Id = id, Name = "UpdatedName", Price = 150, Stock = 15, Color = "Purple" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingProduct);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var (factory, client) = CreateClient();

            await using (factory)
            {
                // Act
                var response = await client.PutAsJsonAsync("/api/Products", updatedProduct);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);


                _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
                // UpdateAsync mevcut entity ile çağrıldı mı?
                _mockRepository.Verify(r => r.UpdateAsync(It.Is<Product>(p => ReferenceEquals(p, existingProduct))), Times.Once);

                // Mevcut entity'nin alanları gerçekten güncellendi mi?
                Assert.Equal(updatedProduct.Name, existingProduct.Name);
                Assert.Equal(updatedProduct.Price, existingProduct.Price);
                Assert.Equal(updatedProduct.Stock, existingProduct.Stock);
                Assert.Equal(updatedProduct.Color, existingProduct.Color);

            }

        }
    }
}

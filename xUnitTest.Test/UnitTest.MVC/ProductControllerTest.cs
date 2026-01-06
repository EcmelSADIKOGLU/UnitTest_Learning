using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using UnitTest.MVC.Web.Controllers;
using UnitTest.MVC.Web.Models;
using UnitTest.MVC.Web.Repository;

namespace xUnitTest.Test.UnitTest.MVC;

public class ProductControllerTest
{
    private readonly Mock<IRepository<Product>> _mockRepository;
    private readonly ProductsController _controller;
    private readonly IEnumerable<Product> _products;
    public ProductControllerTest()
    {
        _mockRepository = new Mock<IRepository<Product>>();
        _controller = new ProductsController(_mockRepository.Object);

        _products = new List<Product>()
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
    public async Task Index_NoProducts_ReturnsViewWithEmptyList()
    {

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Index_SimpleCall_ReturnsViewWithAListOfProducts()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_products);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            
        Assert.Equal(_products.Count(), model.Count());
    }

    [Theory]
    [InlineData(null)]
    public async Task Details_IdIsNull_ReturnsNotFoundResult(int? id)
    {
        var result = await _controller.Details(id);
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(0)]
    public async Task Details_NotExistProduct_ReturnsNotFoundResult(int? id)
    {
        var result = await _controller.Details(id);
        Assert.IsType<NotFoundResult>(result);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Details_ExistProduct_ReturnsViewWithProduct(int id) 
    {
        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(_products.FirstOrDefault(x=> x.Id == id));

        var result = await _controller.Details(id);
        var viewResult =  Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Product>(viewResult.Model);

    }

}

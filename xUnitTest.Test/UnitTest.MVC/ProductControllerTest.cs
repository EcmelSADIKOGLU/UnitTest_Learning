using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
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
    public async Task Details_IdIsNull_RedirectToIndexAction(int? id)
    {
        // Act
        var result = await _controller.Details(id);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Theory]
    [InlineData(0)]
    public async Task Details_NotExistProductId_ReturnsNotFoundResult(int? id)
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(id!.Value)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.Details(id);

        // Assert
        var redirect = Assert.IsType<NotFoundResult>(result);
        Assert.Equal<int>(404, redirect.StatusCode);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Details_ExistProduct_ReturnsViewWithProduct(int? id) 
    {
        // Arrange
        var product = _products.First(x => x.Id == id);
        _mockRepository.Setup(repo => repo.GetByIdAsync(id!.Value)).ReturnsAsync(product);

        // Act
        var result = await _controller.Details(id);

        // Assert
        var viewResult =  Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.Model);

        Assert.Equal(product.Id, model.Id);
        Assert.Equal(product.Name, model.Name);
        Assert.Equal(product.Price, model.Price);

    }

    [Fact]
    public void Create_GetRequest_ReturnsViewResult()
    {
        // Act
        var result = _controller.Create();

        // Assert
        var redirect = Assert.IsType<ViewResult>(result);
        Assert.Null(redirect.Model);
    }

    [Fact]
    public async Task Create_PostRequest_InvalidModelState_ReturnsViewWithProduct()
    {
        // Arrange
        var exampleProduct = _products.First();

        _controller.ModelState.AddModelError("Name", "The Name field is required.");

        // Act
        var result = await _controller.Create(exampleProduct);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
        Assert.Equal(exampleProduct, model);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }


    [Fact]
    public async Task Create_PostRequest_ValidModelState_RedirectToIndexAction()
    {

        // Act
        var result = await _controller.Create(_products.First());

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(_controller.Index), redirect.ActionName);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public async Task Edit_GetRequest_NullId_RedirectToIndexAction()
    {
        // Arrange
        int? id = null;

        //Act
        var result = await _controller.Edit(id);

        //Assert

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(_controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task Edit_GetRequest_NotExistProductId_ReturnsNotFoundResult()
    {
        //Arrange
        int id = 0;
        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        //Act
        var result = await _controller.Edit(id);

        //Assert
        var notFound = Assert.IsType<NotFoundResult>(result);
        Assert.Equal<int>(404, notFound.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Edit_GetRequest_ExistProduct_ReturnsViewWithProduct(int id)
    {
        //Arrange
        Product product = _products.First(x => x.Id == id);
        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);

        //Act
        var result = await _controller.Edit(id);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.Model);

        Assert.Equal(product.Id, model.Id);
        Assert.Equal(product.Name, model.Name);
        Assert.Equal(product.Price, model.Price);

    }

    [Fact]
    public async Task Edit_PostRequest_WrongProductId_ReturnsNotFoundResult()
    {
        //Arrange
        int id = 1;
        Product product = _products.First(x => x.Id == 2);

        //Act
        var response = await _controller.Edit(id, product);

        //Assert
        var notFound = Assert.IsType<NotFoundResult>(response);
        Assert.Equal<int>(404, notFound.StatusCode);
    }

    [Fact]
    public async Task Edit_PostRequest_InvalidModelState_ReturnsViewWithProduct()
    {
        //Arrange
        int id = 1;
        Product product = _products.First(x => x.Id == 1);

        _controller.ModelState.AddModelError("Name", "Example_ModelState_Error");

        //Act
        var response = await _controller.Edit(id, product);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(response);
        var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
        Assert.Equal<int>(product.Id, model.Id);

        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Edit_PostRequest_ValidModelStateWithError_ProductNotExist_ReturnNotFound()
    {
        //Arrange
        int id = 1;
        Product product = _products.First(x => x.Id == 1);
        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Product?)null);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ThrowsAsync(new DbUpdateConcurrencyException());

        //Act
        var response = await _controller.Edit(id, product);

        //Assert
        var notFound = Assert.IsType<NotFoundResult>(response);
        Assert.Equal<int>(404, notFound.StatusCode);
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Edit_PostRequest_ValidModelStateWithError_ProductExist_ThrowException()
    {
        //Arrange
        int id = 1;
        Product product = _products.First(x => x.Id == 1);
        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ThrowsAsync(new DbUpdateConcurrencyException());

        //Act - Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, product));
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Edit_PostRequest_ValidModelStateWithoutError_RedirectToIndexAction()
    {
        //Arrange
        int id = 1;
        Product product = _products.First(x => x.Id == 1);

        //Act
        var response = await _controller.Edit(id, product);

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(response);
        Assert.Equal(nameof(_controller.Index), redirect.ActionName);
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }






}

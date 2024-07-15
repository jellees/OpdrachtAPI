using Moq;
using OpdrachtAPI.Controllers;
using OpdrachtAPI.Models;
using OpdrachtAPI.Services;
using OpenTelemetry.Metrics;

namespace OpdrachtAPI.Tests;

public class ProductsControllerTests
{
    readonly Mock<IProductService> _productService;
    readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productService = new();

        var metrics = new Mock<IOpdrachtAPIMetrics>();
        var service = new Mock<IServiceWrapper>();
        service.Setup(m => m.Products).Returns(_productService.Object);

        _controller = new(service.Object, metrics.Object);
    }

    [Fact]
    public async Task PostProduct_UseValidInput_IsAdded()
    {
        // Arrange
        var product = new Product() { Description = "New product", Price = 2.0 };

        // Act
        await _controller.PostProduct(product);

        // Assert
        _productService.Verify(m => m.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}
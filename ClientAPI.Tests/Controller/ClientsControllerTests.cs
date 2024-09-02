using Moq;
using ClientAPI.Application.Interface;
using ClientAPI.Application.DTOs;
using ClientAPI.API.Controllers;
using Microsoft.AspNetCore.Mvc;


public class ClientsControllerTests
{
    private readonly ClientsController _controller;
    private readonly Mock<IClientService> _mockClientService;

    public ClientsControllerTests()
    {
        _mockClientService = new Mock<IClientService>();
        _controller = new ClientsController(_mockClientService.Object);
    }

  [Fact]
public async Task GetAll_ReturnsOkResult_WithListOfClients()
{
    // Arrange
    var mockClientService = new Mock<IClientService>();
    mockClientService.Setup(service => service.GetAllAsync())
        .ReturnsAsync(new List<ClientDto> { new ClientDto { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" } });
    var controller = new ClientsController(mockClientService.Object);

    // Act
    var result = await controller.GetAll();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnValue = Assert.IsType<List<ClientDto>>(okResult.Value);
    Assert.Single(returnValue);
}

[Fact]
public async Task GetById_ReturnsOkResult_WithClient()
{
    // Arrange
    var clientId = Guid.NewGuid();
    var mockClientService = new Mock<IClientService>();
    mockClientService.Setup(service => service.GetByIdAsync(clientId))
        .ReturnsAsync(new ClientDto { Id = clientId, Name = "Client1", Size = "Large" });
    var controller = new ClientsController(mockClientService.Object);

    // Act
    var result = await controller.GetById(clientId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnValue = Assert.IsType<ClientDto>(okResult.Value);
    Assert.Equal(clientId, returnValue.Id);
}

[Fact]
public async Task Create_ReturnsClientDto()
{
    // Arrange
    var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
    var mockClientService = new Mock<IClientService>();
    mockClientService.Setup(service => service.AddAsync(clientDto))
        .ReturnsAsync(new ClientDto { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" });
    var controller = new ClientsController(mockClientService.Object);

    // Act
    var result = await controller.Create(clientDto);

    // Assert
    Assert.IsType<ClientDto>(result);
}

[Fact]
public async Task Update_ReturnsOkResult_WithUpdatedClient()
{
    // Arrange
    var clientId = Guid.NewGuid();
    var clientDto = new ClientDto { Name = "UpdatedClient", Size = "Medium" };
    var mockClientService = new Mock<IClientService>();
    mockClientService.Setup(service => service.UpdateAsync(clientId, clientDto))
        .ReturnsAsync(new ClientDto { Id = clientId, Name = "UpdatedClient", Size = "Medium" });
    var controller = new ClientsController(mockClientService.Object);

    // Act
    var result = await controller.Update(clientId, clientDto);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnValue = Assert.IsType<ClientDto>(okResult.Value);
    Assert.Equal(clientId, returnValue.Id);
    Assert.Equal("UpdatedClient", returnValue.Name);
}

[Fact]
public async Task Delete_ReturnsNoContentResult()
{
    // Arrange
    var clientId = Guid.NewGuid();
    var mockClientService = new Mock<IClientService>();
    mockClientService.Setup(service => service.DeleteAsync(clientId))
        .Returns(Task.CompletedTask);
    var controller = new ClientsController(mockClientService.Object);

    // Act
    var result = await controller.Delete(clientId);

    // Assert
    Assert.IsType<NoContentResult>(result);
}
}
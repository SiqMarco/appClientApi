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
        var clients = new List<ClientDto> { new ClientDto { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" } };
        _mockClientService.Setup(service => service.GetAllAsync()).ReturnsAsync(clients);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ClientDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithClient()
    {
        var clientId = Guid.NewGuid();
        var client = new ClientDto { Id = clientId, Name = "Client1", Size = "Large" };
        _mockClientService.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync(client);

        var result = await _controller.GetById(clientId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClientDto>(okResult.Value);
        Assert.Equal(clientId, returnValue.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        _mockClientService.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync((ClientDto)null);

        var result = await _controller.GetById(clientId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithCreatedClient()
    {
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        var createdClient = new ClientDto { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" };
        _mockClientService.Setup(service => service.AddAsync(clientDto)).ReturnsAsync(createdClient);

        var result = await _controller.Create(clientDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<ClientDto>(createdAtActionResult.Value);
        Assert.Equal(createdClient.Id, returnValue.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenClientDtoIsNull()
    {
        var result = await _controller.Create(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WithUpdatedClient()
    {
        var clientId = Guid.NewGuid();
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        var updatedClient = new ClientDto { Id = clientId, Name = "Client1", Size = "Large" };
        _mockClientService.Setup(service => service.UpdateAsync(clientId, clientDto)).ReturnsAsync(updatedClient);

        var result = await _controller.Update(clientId, clientDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClientDto>(okResult.Value);
        Assert.Equal(clientId, returnValue.Id);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        _mockClientService.Setup(service => service.UpdateAsync(clientId, clientDto)).ReturnsAsync((ClientDto)null);

        var result = await _controller.Update(clientId, clientDto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenClientDtoIsNull()
    {
        var clientId = Guid.NewGuid();

        var result = await _controller.Update(clientId, null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenClientIsDeleted()
    {
        var clientId = Guid.NewGuid();
        var client = new ClientDto { Id = clientId, Name = "Client1", Size = "Large" };
        _mockClientService.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync(client);
        _mockClientService.Setup(service => service.DeleteAsync(clientId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(clientId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        _mockClientService.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync((ClientDto)null);

        var result = await _controller.Delete(clientId);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
using Moq;
using ClientAPI.Application.DTOs;
using ClientAPI.Domain.Entities;
using ClientAPI.Application.Services;
using ClientAPI.Domain.Repositories;

namespace ClientAPI.Tests.Services;

public class ClientServicesTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly ClientService _service;

    public ClientServicesTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _service = new ClientService(_mockClientRepository.Object);
    }

[Fact]
public async Task GetAllAsync_ReturnsListOfClientDtos()
{
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.GetAllAsync())
        .ReturnsAsync(new List<Client> { new Client { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" } });
    var service = new ClientService(mockRepository.Object);

    var result = await service.GetAllAsync();

    Assert.Single(result);
    Assert.Equal("Client1", result.First().Name);
}

[Fact]
public async Task GetByIdAsync_ReturnsClientDto_WhenClientExists()
{
    var clientId = Guid.NewGuid();
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.GetByIdAsync(clientId))
        .ReturnsAsync(new Client { Id = clientId, Name = "Client1", Size = "Large" });
    var service = new ClientService(mockRepository.Object);

    var result = await service.GetByIdAsync(clientId);

    Assert.Equal(clientId, result.Id);
    Assert.Equal("Client1", result.Name);
}

[Fact]
public async Task GetByIdAsync_ThrowsInvalidOperationException_WhenClientDoesNotExist()
{
    var clientId = Guid.NewGuid();
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.GetByIdAsync(clientId))
        .ThrowsAsync(new InvalidOperationException());
    var service = new ClientService(mockRepository.Object);

    await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetByIdAsync(clientId));
}

[Fact]
public async Task AddAsync_AddsClientAndReturnsClientDto()
{
    var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
    var client = new Client { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" };
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Client>()))
        .ReturnsAsync(client);
    var service = new ClientService(mockRepository.Object);

    var result = await service.AddAsync(clientDto);

    Assert.Equal(client.Id, result.Id);
    Assert.Equal("Client1", result.Name);
}

[Fact]
public async Task UpdateAsync_UpdatesClientAndReturnsClientDto()
{
    var clientId = Guid.NewGuid();
    var clientDto = new ClientDto { Name = "UpdatedClient", Size = "Medium" };
    var client = new Client { Id = clientId, Name = "UpdatedClient", Size = "Medium" };
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>()))
        .ReturnsAsync(client);
    var service = new ClientService(mockRepository.Object);

    var result = await service.UpdateAsync(clientId, clientDto);

    Assert.Equal(clientId, result.Id);
    Assert.Equal("UpdatedClient", result.Name);
}

[Fact]
public async Task DeleteAsync_DeletesClient()
{
    var clientId = Guid.NewGuid();
    var mockRepository = new Mock<IClientRepository>();
    mockRepository.Setup(repo => repo.DeleteAsync(clientId))
        .Returns(Task.CompletedTask);
    var service = new ClientService(mockRepository.Object);

    await service.DeleteAsync(clientId);

    mockRepository.Verify(repo => repo.DeleteAsync(clientId), Times.Once);
}
}
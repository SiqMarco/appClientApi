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
        var clients = new List<Client> { new Client { Id = Guid.NewGuid(), Name = "Client1", Size = "Large" } };
        _mockClientRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(clients);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsClientDto_WhenClientExists()
    {
        var clientId = Guid.NewGuid();
        var client = new Client { Id = clientId, Name = "Client1", Size = "Large" };
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(clientId)).ReturnsAsync(client);

        var result = await _service.GetByIdAsync(clientId);

        Assert.NotNull(result);
        Assert.Equal(clientId, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(clientId)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(clientId));
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedClientDto()
    {
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        var client = new Client { Id = Guid.NewGuid(), Name = clientDto.Name, Size = clientDto.Size };
        _mockClientRepository.Setup(repo => repo.AddAsync(It.IsAny<Client>())).ReturnsAsync(client);

        var result = await _service.AddAsync(clientDto);

        Assert.NotNull(result);
        Assert.Equal(client.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedClientDto_WhenClientExists()
    {
        var clientId = Guid.NewGuid();
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        var client = new Client { Id = clientId, Name = clientDto.Name, Size = clientDto.Size };
        _mockClientRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>())).ReturnsAsync(client);

        var result = await _service.UpdateAsync(clientId, clientDto);

        Assert.NotNull(result);
        Assert.Equal(clientId, result.Id);
        Assert.Equal(clientDto.Name, result.Name);
        Assert.Equal(clientDto.Size, result.Size);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = Guid.NewGuid();
        var clientDto = new ClientDto { Name = "Client1", Size = "Large" };
        _mockClientRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>())).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(clientId, clientDto));
    }

    [Fact]
    public async Task DeleteAsync_DeletesClient()
    {
        var clientId = Guid.NewGuid();
        _mockClientRepository.Setup(repo => repo.DeleteAsync(clientId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(clientId);

        _mockClientRepository.Verify(repo => repo.DeleteAsync(clientId), Times.Once);
    }
}
using ClientAPI.Domain.Entities;
using ClientAPI.Infrastructure.Data;
using ClientAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ClientAPI.Tests.Repositories
{
    public class ClientRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ClientRepository _repository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new ClientRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { Id = Guid.NewGuid(), Name = "Client 1", Size = "Pequena" },
                new Client { Id = Guid.NewGuid(), Name = "Client 2", Size = "Média" }
            };

            _context.Clients.AddRange(clients);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsClient()
        {
            // Arrange
            var client = new Client { Id = Guid.NewGuid(), Name = "Client 1", Size = "Pequena" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(client.Id);

            // Assert
            Assert.Equal(client.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ThrowsInvalidOperationException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await _repository.GetByIdAsync(nonExistingId));
        }

        [Fact]
        public async Task AddAsync_ValidClient_AddsClient()
        {
            // Arrange
            var client = new Client { Name = "New Client", Size = "Média" };

            // Act
            var result = await _repository.AddAsync(client);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Name, result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ValidClient_UpdatesClient()
        {
            // Arrange
            var client = new Client { Id = Guid.NewGuid(), Name = "Client 1", Size = "Pequena" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            client.Name = "Updated Client";

            // Act
            var result = await _repository.UpdateAsync(client);

            // Assert
            Assert.Equal(client.Name, result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_RemovesClient()
        {
            // Arrange
            var client = new Client { Id = Guid.NewGuid(), Name = "Client to Delete", Size = "Pequena" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(client.Id);

            // Assert
            var result = await _context.Clients.FindAsync(client.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_DoesNothing()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            await _repository.DeleteAsync(nonExistingId);

            // Assert
            // Ensure that no exception is thrown and method completes
            await Task.CompletedTask;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ClientAPI.Domain.Entities;
using ClientAPI.Infrastructure.Data;
using ClientAPI.Infrastructure.Repositories;
using Xunit;

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
            await CleanContext();
            _context.Clients.Add(new Client { Id = Guid.NewGuid(), Name = "Client 1", Size = "grande"});
            _context.Clients.Add(new Client { Id = Guid.NewGuid(), Name = "Client 2", Size = "pequena"});
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal("Client 1", result.First().Name);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ClientExists_ReturnsClient()
        {
            var clientId = Guid.NewGuid();
            var client = new Client { Id = clientId, Name = "Test Client", Size = "media"};
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(clientId);

            Assert.NotNull(result);
            Assert.Equal(clientId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ClientDoesNotExist_ThrowsInvalidOperationException()
        {
            var clientId = Guid.NewGuid();
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetByIdAsync(clientId));
        }

        [Fact]
        public async Task AddAsync_ValidClient_ReturnsClientWithId()
        {
            await CleanContext();
            var client = new Client { Id = Guid.NewGuid(), Name = "New Client", Size = "grande"};
            var result = await _repository.AddAsync(client);

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
            Assert.Equal(1, _context.Clients.Count());
        }

        [Fact]
        public async Task UpdateAsync_ValidClient_ReturnsUpdatedClient()
        {
            var client = new Client { Id = Guid.NewGuid(), Name = "Client 1" , Size = "grande"};
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            client.Name = "Updated Client";
            var result = await _repository.UpdateAsync(client);

            Assert.NotNull(result);
            Assert.Equal("Updated Client", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ClientExists_RemovesClient()
        {
            var clientId = Guid.NewGuid();
            var client = new Client { Id = clientId, Name = "Test Client", Size = "grande"};
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(clientId);

            Assert.Equal(0, _context.Clients.Count());
        }
        
        private async Task CleanContext()
        {
            _context.Clients.RemoveRange(_context.Clients);
            await _context.SaveChangesAsync();
        }
    }
}
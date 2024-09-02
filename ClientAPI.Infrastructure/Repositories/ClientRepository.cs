using ClientAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using ClientAPI.Domain.Entities;
using ClientAPI.Infrastructure.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace ClientAPI.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client> GetByIdAsync(Guid id)
    {
        try
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                throw new InvalidOperationException($"Falha ao buscar o id {id}, cliente não encontrado.");
            }
            return client;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<Client> AddAsync(Client client)
    {
        try
        {
            await _context.AddAsync(client);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(client.Id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Falha ao salvar novo cliente. " + ex.Message);
        }
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        try
        {
            _context.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException($"Falha na atualização do Id {client.Id}." + ex.Message);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error na atualização do Id {client.Id}" + ex.Message);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {   
            var client = await GetByIdAsync(id);
            _context.Remove(client);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Falha ao excluir o ID: {id}. " + ex.Message);
        }

    }
    
}

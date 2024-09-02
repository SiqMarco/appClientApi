using System.Net;
using ClientAPI.Application.DTOs;
using ClientAPI.Application.Interface;
using ClientAPI.Domain.Entities;
using ClientAPI.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.Select(client => new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            Size = client.Size
        });
    }

    public async Task<ClientDto> GetByIdAsync(Guid id)
    {
            var client = await _clientRepository.GetByIdAsync(id);
            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Size = client.Size
            };
            
    }
    

    public async Task<ClientDto> AddAsync(ClientDto clientDto)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = clientDto.Name,
            Size = clientDto.Size
        };
        var addedClient = await _clientRepository.AddAsync(client);
        return new ClientDto
        {
            Id = addedClient.Id,
            Name = addedClient.Name,
            Size = addedClient.Size
        };
    }

    public async Task<ClientDto> UpdateAsync(Guid id, ClientDto clientDto)
    {
        var client = new Client
        {
            Id = id,
            Name = clientDto.Name,
            Size = clientDto.Size
        };
        var updatedClient = await _clientRepository.UpdateAsync(client);
        return new ClientDto
        {
            Id = updatedClient.Id,
            Name = updatedClient.Name,
            Size = updatedClient.Size
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await _clientRepository.DeleteAsync(id);
    }
    
}
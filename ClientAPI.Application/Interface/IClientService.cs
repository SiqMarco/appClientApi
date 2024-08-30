using ClientAPI.Application.DTOs;

namespace ClientAPI.Application.Interface
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto> AddAsync(ClientDto clientDto);
        Task<ClientDto> UpdateAsync(Guid id, ClientDto clientDto);
        Task<ClientDto> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
using ClientAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientAPI.Domain.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task <Client>AddAsync(Client client);
    Task  <Client>UpdateAsync(Client client);
    Task DeleteAsync(Guid id);
    Task<Client> GetByIdAsync(Guid id);
}
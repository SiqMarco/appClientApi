using Microsoft.AspNetCore.Mvc;
using ClientAPI.Application.DTOs;
using ClientAPI.Application.Interface;

namespace ClientAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _clientService.GetAllAsync();
        return Ok(clients);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var client = await _clientService.GetByIdAsync(id);
        return Ok(client);
    }

    [HttpPost]
    public async Task<ClientDto> Create([FromBody] ClientDto clientDto)
    {
        return await _clientService.AddAsync(clientDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ClientDto clientDto)
    {
        var updatedClient = await _clientService.UpdateAsync(id, clientDto);
        return Ok(updatedClient);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _clientService.DeleteAsync(id);
        return NoContent();
    }
}

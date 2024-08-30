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
        if (client == null)
            return NotFound(new { Message = "Client not found" });

        return Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
    {
        if (clientDto == null)
            return BadRequest(new { Message = "Invalid client data" });

        var createdClient = await _clientService.AddAsync(clientDto);
        return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ClientDto clientDto)
    {
        if (clientDto == null)
            return BadRequest(new { Message = "Invalid client data" });

        var updatedClient = await _clientService.UpdateAsync(id, clientDto);
        if (updatedClient == null)
            return NotFound(new { Message = "Client not found" });

        return Ok(updatedClient);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = await _clientService.GetByIdAsync(id);
        if (client == null)
            return NotFound(new { Message = "Client not found" });

        await _clientService.DeleteAsync(id);
        return NoContent();
    }
}

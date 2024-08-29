using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientAPI.Application.DTOs;
using ClientAPI.Application.Services;

namespace ClientAPI.API.Pages.Clients;

public class IndexModel(ClientService clientService, IList<ClientDto> clients) : PageModel
{
    public IList<ClientDto> Clients { get; set; } = clients;

    public async Task OnGetAsync()
    {
        Clients = (await clientService.GetAllAsync()).ToList();
    }
}
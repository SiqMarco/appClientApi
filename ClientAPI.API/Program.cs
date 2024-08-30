using ClientAPI.Application.Interface;
using ClientAPI.Application.Services;
using ClientAPI.Infrastructure.Data;
using ClientAPI.Domain.Repositories; // Certifique-se de que o namespace está correto
using ClientAPI.Infrastructure.Repositories; // O namespace da implementação concreta
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionando a configuração de Razor Pages com recompilação em tempo de execução
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Adicionando o contexto de banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 26))));

// Registrar o repositório junto com sua interface
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Registrar o serviço junto com sua interface
builder.Services.AddScoped<IClientService, ClientService>();

// Adicionar o serviço ClientService ao container de DI
builder.Services.AddScoped<ClientService>();

// Adicionar suporte para controladores (API)
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles(); // Serve os arquivos estáticos do React
app.UseRouting();
app.UseHttpsRedirection();

// Use o middleware CORS antes da autorização
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html"); // Serve o React app

app.Run();
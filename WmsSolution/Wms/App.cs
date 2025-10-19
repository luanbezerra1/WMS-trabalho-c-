/**
 * Autor: Vitor, Caua, Luan
 * Data de Criação: 08/10/2025
 * Descrição: Arquivo principal da aplicação WMS
**/

using Microsoft.EntityFrameworkCore;
using Wms.Models;
using Wms.Controllers;

var builder = WebApplication.CreateBuilder(args);

// ▶ DB: registra seu DbContext com SQLite (usa appsettings.json)
builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ▶ API + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configurar JSON para Minimal API (endpoints)
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- LOGGING (antes do Build!)
builder.Logging.ClearProviders();   // limpa provedores padrão
builder.Logging.AddConsole();       // console já resolve seu endpoint de logs

var app = builder.Build();


// ▶ Swagger no Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ▶ Endpoint de teste + seus controllers
app.MapGet("/", () => "API WMS rodando");
app.MapControllers();

// ▶ Registra os endpoints de Endereço
app.MapEndpointsEndereco();

// ▶ Registra os endpoints de Cliente
app.MapEndpointsCliente();

// ▶ Registra os endpoints de Fornecedor
app.MapEndpointsFornecedor();

// ▶ Registra os endpoints de Produto
app.MapEndpointsProduto();

// ▶ Registra os endpoints de Usuário
app.MapEndpointsUsuario();

// ▶ Registra os endpoints de Armazém
app.MapEndpointsArmazem();

// ▶ Registra os endpoints de Inventário
app.MapEndpointsInventario();

// ▶ Registra os endpoints de Entrada de Produto
app.MapEndpointsEntradaProduto();

<<<<<<< HEAD

        
=======
// ▶ Registra os endpoints de Logs
app.MapEndpointsLogs();
>>>>>>> master

app.Run();

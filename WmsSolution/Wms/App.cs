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

// ▶ CORS: Acesso Total (conforme solicitado)
builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

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

var app = builder.Build();

// ▶ Garantir que o banco exista e esteja migrado
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();
    db.Database.Migrate();
}

// ▶ Swagger no Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Acesso Total");
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

// ▶ Registra os endpoints de Saida de Produto
app.MapEndpointsSaidaProduto();

// ▶ Registra os endpoints de Logs
app.MapEndpointsLogs();

app.Run();

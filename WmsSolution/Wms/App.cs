/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Arquivo principal da aplicação WMS
**/

using Microsoft.EntityFrameworkCore;
using Wms.Models;

var builder = WebApplication.CreateBuilder(args);

// ▶ DB: registra seu DbContext com SQLite (usa appsettings.json)
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ▶ API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ▶ Swagger no Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// endpoint de teste + seus controllers
app.MapGet("/", () => "API WMS rodando");
app.MapControllers();

app.Run();

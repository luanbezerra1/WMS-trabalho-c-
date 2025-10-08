/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Arquivo principal da aplicação WMS
**/

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

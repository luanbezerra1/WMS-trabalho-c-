/**
 * Autor: Cauã Tobias
 * Data de Criação: 14/10/2025
 * Descrição: Endpoints responsáveis pelo gerenciamento de Armazéns (CRUD)
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Endpoints
{
    public static class EndpointsArmazem
    {
        public static void MapArmazemEndpoints(this WebApplication app)
        {
            /**
             * Autor: Cauã Tobias
             * Data de Criação: 14/10/2052
             * Descrição: Endpoint para listar todos os armazéns
            **/
            app.MapGet("/api/GetArmazem", ([FromServices] AppDataContext context) =>
            {
                 var armazens = context.Armazem.ToList();
                 return Results.Ok(armazens);
            });

            /**
             * Autor: Cauã Tobias
             * Data de Criação: 14/10/2025
             * Descrição: Endpoint para buscar um armazém por ID
            **/
            app.MapGet("/api/GetArmazemById={id}", ([FromServices] AppDataContext context, int id) =>
            {
                var armazem = context.Armazem.Find(id);

                if (armazem is null)
                    return Results.NotFound("Armazém não encontrado.");

                return Results.Ok(armazem);
            });

            /**
             * Autor: Cauã Tobias
             * Data de Criação: 14/10/2025
             * Descrição: Endpoint para criar um novo armazém
            **/
            app.MapPost("/api/PostArmazem", ([FromServices] AppDataContext context, [FromBody] Armazem armazem) =>
            {
                context.Armazem.Add(armazem);
                context.SaveChanges();

                return Results.Created($"/api/GetArmazemById={armazem.Id}", armazem);
            });

            /**
             * Autor: Cauã Tobias
             * Data de Criação: 14/10/2025
             * Descrição: Endpoint para atualizar um armazém existente
            **/
            app.MapPut("/api/PutArmazem/{id}", ([FromServices] AppDataContext context, int id, [FromBody] Armazem armazemAtualizado) =>
            {
                var armazem = context.Armazem.Find(id);

                if (armazem is null)
                    return Results.NotFound("Armazém não encontrado.");

                armazem.nomeArmazem = armazemAtualizado.nomeArmazem;
                armazem.status = armazemAtualizado.status;
                armazem.Posicoes = armazemAtualizado.Posicoes;
                armazem.ProdutoPosicao = armazemAtualizado.ProdutoPosicao;
                armazem.Capacidade = armazemAtualizado.Capacidade;
                armazem.EnderecoId = armazemAtualizado.EnderecoId;

                context.SaveChanges();

                return Results.Ok(armazem);
            });

            /**
             * Autor: Cauã Tobias
             * Data de Criação: 14/10/2025
             * Descrição: Endpoint para excluir um armazém
            **/
            app.MapDelete("/api/DeleteArmazem/{id}", ([FromServices] AppDataContext context, int id) =>
            {
                var armazem = context.Armazem.Find(id);

                if (armazem is null)
                    return Results.NotFound("Armazém não encontrado.");

                context.Armazem.Remove(armazem);
                context.SaveChanges();

                return Results.Ok("Armazém removido com sucesso.");
            });
        }
    }
}

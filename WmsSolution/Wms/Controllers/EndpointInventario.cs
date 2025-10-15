/**
 * Autor: Cauã Tobias
 * Data de Criação: 14/10/2025
 * Descrição: Endpoints para CRUD de Inventário (Atualizado)
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointInventario
    {
        public static void MapEndpointsInventario(this WebApplication app)
        {
            /*
            Autor: Cauã Tobias
            Data de Criação: 14/10/2025
            Descrição: Endpoints para CRUD de Inventário.
            Args: ctx(AppDataContext)
            */

            app.MapGet("/api/GetInventario", ([FromServices] AppDataContext ctx) =>
            {
                /*
                Autor: Cauã Tobias
                Descrição: Endpoint Get para listar todos os inventários.
                */
                if (ctx.Inventario.Any())
                {
                    return Results.Ok(ctx.Inventario.ToList());
                }

                return Results.NotFound("Nenhum inventário encontrado!");
            });

            app.MapGet("/api/GetInventarioByArmazemProduto={armazemId}/{produtoId}", 
                ([FromRoute] int armazemId, [FromRoute] int produtoId, [FromServices] AppDataContext ctx) =>
            {
                /*
                Autor: Cauã Tobias
                Descrição: Endpoint Get para buscar um inventário por ArmazemId e ProdutoId.
                */
                var resultado = ctx.Inventario.FirstOrDefault(x => x.ArmazemId == armazemId && x.ProdutoId == produtoId);

                if (resultado is null)
                {
                    return Results.NotFound("Inventário não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostInventario", ([FromBody] Inventario inventario, [FromServices] AppDataContext ctx) =>
            {
                /*
                Autor: Cauã Tobias
                Descrição: Endpoint Post para cadastrar um inventário.
                */
                var existente = ctx.Inventario.FirstOrDefault(x => x.ArmazemId == inventario.ArmazemId && x.ProdutoId == inventario.ProdutoId);

                if (existente is not null)
                {
                    return Results.Conflict("Esse inventário já existe!");
                }

                ctx.Inventario.Add(inventario);
                ctx.SaveChanges();

                return Results.Created($"/api/GetInventarioByArmazemProduto={inventario.ArmazemId}/{inventario.ProdutoId}", inventario);
            });

            app.MapPut("/api/PutInventario={armazemId}/{produtoId}", 
                ([FromRoute] int armazemId, [FromRoute] int produtoId, [FromBody] Inventario inventarioAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*
                Autor: Cauã Tobias
                Descrição: Endpoint Put para alterar um inventário.
                */
                var resultado = ctx.Inventario.FirstOrDefault(x => x.ArmazemId == armazemId && x.ProdutoId == produtoId);

                if (resultado is null)
                {
                    return Results.NotFound("Inventário não encontrado!");
                }

                resultado.Quantidade = inventarioAlterado.Quantidade;
                resultado.ultimaMovimentacao = inventarioAlterado.ultimaMovimentacao;

                ctx.Inventario.Update(resultado);
                ctx.SaveChanges();

                return Results.Ok(resultado);
            });

            app.MapDelete("/api/DeleteInventario={armazemId}/{produtoId}", 
                ([FromRoute] int armazemId, [FromRoute] int produtoId, [FromServices] AppDataContext ctx) =>
            {
                /*
                Autor: Cauã Tobias
                Descrição: Endpoint Delete para deletar um inventário.
                */
                var resultado = ctx.Inventario.FirstOrDefault(x => x.ArmazemId == armazemId && x.ProdutoId == produtoId);

                if (resultado is null)
                {
                    return Results.NotFound("Inventário não encontrado!");
                }

                ctx.Inventario.Remove(resultado);
                ctx.SaveChanges();

                return Results.Ok(resultado);
            });
        }
    }
}

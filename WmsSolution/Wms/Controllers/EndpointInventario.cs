/**
 * Autor: Caua
 * Data de Criação: 15/10/2025
 * Descrição: Endpoints para CRUD de Inventário (Gerenciamento de Posições)
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

            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Endpoints para CRUD de Inventário (Posições).
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetInventario", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para listar todos os inventários.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Inventario.ToList()) ou Results.NotFound("Nenhum inventário encontrado!")

                */

                if (ctx.Inventario.Any())
                {
                    return Results.Ok(ctx.Inventario.ToList());
                }

                return Results.NotFound("Nenhum inventário encontrado!");
            });

            app.MapGet("/api/GetInventarioById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar um inventário por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Inventário não encontrado!")

                */

                Inventario? resultado = ctx.Inventario.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound("Inventário não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetInventarioByArmazem={armazemId}", ([FromRoute] int armazemId, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar todos os inventários de um armazém.
                Args: armazemId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhum inventário encontrado para esse armazém!")

                */

                var resultado = ctx.Inventario.Where(x => x.ArmazemId == armazemId).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhum inventário encontrado para esse armazém!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetInventarioByProduto={produtoId}", ([FromRoute] int produtoId, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar todos os inventários de um produto.
                Args: produtoId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhum inventário encontrado para esse produto!")

                */

                var resultado = ctx.Inventario.Where(x => x.ProdutoId == produtoId).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhum inventário encontrado para esse produto!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetPosicoesVazias={armazemId}", ([FromRoute] int armazemId, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar posições vazias de um armazém.
                Args: armazemId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhuma posição vazia encontrada!")

                */

                var resultado = ctx.Inventario.Where(x => x.ArmazemId == armazemId && x.ProdutoId == null).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhuma posição vazia encontrada!");
                }

                return Results.Ok(resultado);
            });

            app.MapPut("/api/AlocarProduto={id}", ([FromRoute] int id, [FromBody] dynamic dados, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para alocar um produto em uma posição vazia.
                Args: id(int), dados(dynamic), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound/BadRequest

                */

                Inventario? posicao = ctx.Inventario.Find(id);

                if (posicao is null)
                {
                    return Results.NotFound("Posição não encontrada!");
                }

                if (posicao.ProdutoId is not null)
                {
                    return Results.BadRequest("Esta posição já está ocupada! Use o endpoint de atualizar quantidade.");
                }

                int produtoId = (int)dados.produtoId;
                int quantidade = (int)dados.quantidade;

                // Valida se o produto existe
                Produto? produtoExistente = ctx.Produto.Find(produtoId);
                if (produtoExistente is null)
                {
                    return Results.NotFound($"Produto com ID {produtoId} não encontrado!");
                }

                // Valida se a quantidade é válida
                if (quantidade <= 0)
                {
                    return Results.BadRequest("Quantidade deve ser maior que zero!");
                }

                posicao.AlocarProduto(produtoId, quantidade);

                ctx.Inventario.Update(posicao);
                ctx.SaveChanges();

                return Results.Ok(posicao);
            });

            app.MapPut("/api/AtualizarQuantidade={id}", ([FromRoute] int id, [FromBody] dynamic dados, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para atualizar quantidade de produto em uma posição.
                Args: id(int), dados(dynamic), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound/BadRequest

                */

                Inventario? posicao = ctx.Inventario.Find(id);

                if (posicao is null)
                {
                    return Results.NotFound("Posição não encontrada!");
                }

                if (posicao.ProdutoId is null)
                {
                    return Results.BadRequest("Esta posição está vazia! Use o endpoint de alocar produto.");
                }

                int quantidade = (int)dados.quantidade;

                // Valida se a quantidade é válida
                if (quantidade < 0)
                {
                    return Results.BadRequest("Quantidade não pode ser negativa!");
                }

                posicao.AtualizarQuantidade(quantidade);

                ctx.Inventario.Update(posicao);
                ctx.SaveChanges();

                return Results.Ok(posicao);
            });

            app.MapPut("/api/RemoverProduto={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para remover produto de uma posição (deixa vazia).
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound/BadRequest

                */

                Inventario? posicao = ctx.Inventario.Find(id);

                if (posicao is null)
                {
                    return Results.NotFound("Posição não encontrada!");
                }

                if (posicao.ProdutoId is null)
                {
                    return Results.BadRequest("Esta posição já está vazia!");
                }

                posicao.RemoverProduto();

                ctx.Inventario.Update(posicao);
                ctx.SaveChanges();

                return Results.Ok(posicao);
            });

            app.MapDelete("/api/DeleteInventario={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Delete para deletar uma posição do inventário.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Posição não encontrada!")

                */

                Inventario? resultado = ctx.Inventario.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Posição não encontrada!");
                }
                
                Inventario.Deletar(ctx, id);

                return Results.Ok(resultado);
            });
        }
    }
}

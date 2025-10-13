/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Endpoints para CRUD de Produto
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointProduto
    {
        public static void MapEndpointsProduto(this WebApplication app)
        {
            /*

            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Endpoints para CRUD de Produto.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetProduto", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para listar todos os produtos.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Produto.ToList()) ou Results.NotFound("Nenhum produto encontrado!")

                */

                if (ctx.Produto.Any())
                {
                    return Results.Ok(ctx.Produto.ToList());
                }

                return Results.NotFound("Nenhum produto encontrado!");
            });

            app.MapGet("/api/GetProdutoById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para buscar um produto por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Produto não encontrado!")

                */

                Produto? resultado = ctx.Produto.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound("Produto não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetProdutoByCategoria={categoria}", ([FromRoute] int categoria, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para buscar produtos por categoria.
                Args: categoria(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhum produto encontrado para essa categoria!")

                */

                var resultado = ctx.Produto.Where(x => (int)x.Categoria == categoria).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhum produto encontrado para essa categoria!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetProdutoByFornecedor={fornecedorId}", ([FromRoute] int fornecedorId, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para buscar produtos por fornecedor.
                Args: fornecedorId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhum produto encontrado para esse fornecedor!")

                */

                var resultado = ctx.Produto.Where(x => x.fornecedorId == fornecedorId).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhum produto encontrado para esse fornecedor!");
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostProduto", ([FromBody] Produto produto, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Post para cadastrar um produto.
                Args: produto(Produto), ctx(AppDataContext)
                Return: Results.Created("", novoProduto) ou Results.Conflict("Esse produto já existe!")

                */

                if (produto.Id != 0)
                {
                    Produto? resultado = ctx.Produto.FirstOrDefault(x => x.Id == produto.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict("Esse produto já existe!");
                    }
                }

                // Valida se o nome do produto foi informado
                if (string.IsNullOrEmpty(produto.nomeProduto))
                {
                    return Results.BadRequest("Nome do produto deve ser informado!");
                }

                // Valida se o fornecedor existe
                Fornecedor? fornecedorExistente = ctx.Fornecedor.Find(produto.fornecedorId);
                if (fornecedorExistente is null)
                {
                    return Results.NotFound($"Fornecedor com ID {produto.fornecedorId} não encontrado!");
                }

                // Valida se o preço é válido
                if (produto.preco <= 0)
                {
                    return Results.BadRequest("Preço do produto deve ser maior que zero!");
                }

                // Valida se a quantidade do lote é válida
                if (produto.lote <= 0)
                {
                    return Results.BadRequest("Lote do produto deve ser maior que zero!");
                }
                
                Produto novoProduto = Produto.Criar(produto.nomeProduto, produto.descricao, produto.lote, produto.fornecedorId, produto.preco, produto.Categoria);
                novoProduto.Id = Produto.GerarId(ctx);
                
                ctx.Produto.Add(novoProduto);
                ctx.SaveChanges();

                return Results.Created("", novoProduto);
            });

            app.MapDelete("/api/DeleteProduto={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Delete para deletar um produto.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Produto não encontrado!")

                */

                Produto? resultado = ctx.Produto.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Produto não encontrado!");
                }
                
                Produto.Deletar(ctx, id);

                return Results.Ok(resultado);
            });

            app.MapPut("/api/PutProduto={id}", ([FromRoute] int id, [FromBody] Produto produtoAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Put para alterar um produto.
                Args: id(int), produtoAlterado(Produto), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Produto não encontrado!")

                */

                Produto? resultado = ctx.Produto.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Produto não encontrado!");
                }

                // Valida se o nome do produto foi informado
                if (string.IsNullOrEmpty(produtoAlterado.nomeProduto))
                {
                    return Results.BadRequest("Nome do produto deve ser informado!");
                }

                // Valida se o fornecedor existe
                Fornecedor? fornecedorExistente = ctx.Fornecedor.Find(produtoAlterado.fornecedorId);
                if (fornecedorExistente is null)
                {
                    return Results.NotFound($"Fornecedor com ID {produtoAlterado.fornecedorId} não encontrado!");
                }

                // Valida se o preço é válido
                if (produtoAlterado.preco <= 0)
                {
                    return Results.BadRequest("Preço do produto deve ser maior que zero!");
                }

                // Valida se a quantidade do lote é válida
                if (produtoAlterado.lote <= 0)
                {
                    return Results.BadRequest("Lote do produto deve ser maior que zero!");
                }
                
                resultado.Alterar(produtoAlterado.nomeProduto, produtoAlterado.descricao, produtoAlterado.lote, produtoAlterado.fornecedorId, produtoAlterado.preco, produtoAlterado.Categoria);

                ctx.Produto.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });
        }
    }
}


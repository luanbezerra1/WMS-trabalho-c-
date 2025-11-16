/**
 * Autor: Vitor, Caua, Luan
 * Data de Criação: 18/10/2025
 * Descrição: Endpoints para controle de entrada de produtos no armazém
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;
using Wms.Enums;

namespace Wms.Controllers
{
    public static class EndpointEntradaProduto
    {
        public static void MapEndpointsEntradaProduto(this WebApplication app)
        {
            /*
            
            Autor: Vitor, Caua, Luan
            Data de Criação: 18/10/2025
            Descrição: Endpoints para controle de entrada de produtos.
            Args: ctx(AppDataContext)
            
            */

            app.MapGet("/api/GetEntradaProduto", ([FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Caua, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Get para listar todas as entradas de produtos.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.EntradaProduto.ToList()) ou Results.NotFound("Nenhuma entrada encontrada!")
                
                */

                if (ctx.EntradaProduto.Any())
                {
                    return Results.Ok(ctx.EntradaProduto.ToList());
                }

                return Results.NotFound(EnumTipoException.ThrowException("MSG0045").Message);
            });

            app.MapGet("/api/GetEntradaProdutoById={entradaId}", ([FromRoute] int entradaId, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Caua, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Get para buscar entradas pelo ID da entrada.
                Args: entradaId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhuma entrada encontrada!")
                
                */

                var resultado = ctx.EntradaProduto
                    .Where(x => x.EntradaId == entradaId)
                    .ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0045").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostEntradaProduto", ([FromBody] System.Text.Json.JsonElement dados, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Caua, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Post para registrar entrada de produto e atualizar inventário.
                Args: dados(JsonElement), ctx(AppDataContext)
                Return: Results.Created ou Results.BadRequest/NotFound
                
                Dados esperados:
                {
                    "fornecedorId": int,
                    "produtoId": int,
                    "quantidadeRecebida": int,
                    "inventarioId": int (ID da posição onde será alocado)
                }
                
                */

                try
                {
                    int fornecedorId = dados.GetProperty("fornecedorId").GetInt32();
                    int produtoId = dados.GetProperty("produtoId").GetInt32();
                    int quantidadeRecebida = dados.GetProperty("quantidadeRecebida").GetInt32();
                    int inventarioId = dados.GetProperty("inventarioId").GetInt32();

                    Fornecedor? fornecedor = ctx.Fornecedor.Find(fornecedorId);
                    if (fornecedor is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0013", fornecedorId).Message);
                    }

                    Produto? produto = ctx.Produto.Find(produtoId);
                    if (produto is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0031", produtoId).Message);
                    }

                    Inventario? posicao = ctx.Inventario.Find(inventarioId);
                    if (posicao is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0032", inventarioId).Message);
                    }

                    if (quantidadeRecebida <= 0)
                    {
                        return Results.BadRequest(EnumTipoException.ThrowException("MSG0034").Message);
                    }

                    Armazem? armazem = ctx.Armazem.Find(posicao.ArmazemId);
                    if (armazem is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0033", posicao.ArmazemId).Message);
                    }

                    if (posicao.ProdutoId.HasValue && posicao.ProdutoId.Value != produtoId)
                    {
                        return Results.BadRequest(EnumTipoException.ThrowException("MSG0063", posicao.NomePosicao, posicao.ProdutoId).Message);
                    }

                    int quantidadeAtual = posicao.Quantidade;
                    int quantidadeTotal = quantidadeAtual + quantidadeRecebida;

                    if (quantidadeTotal > armazem.ProdutoPosicao)
                    {
                        return Results.BadRequest(EnumTipoException.ThrowException("MSG0062",
                            posicao.NomePosicao, armazem.ProdutoPosicao, quantidadeAtual, quantidadeRecebida, quantidadeTotal));
                    }

                    int entradaId = EntradaProduto.GerarEntradaId(ctx);

                    EntradaProduto novaEntrada = EntradaProduto.Criar(entradaId, fornecedorId, produtoId, quantidadeRecebida);

                    ctx.EntradaProduto.Add(novaEntrada);

                    if (posicao.ProdutoId is null)
                    {
                        // posicao.ProdutoId (alocação)
                        posicao.AlocarProduto(produtoId, quantidadeRecebida);
                    }
                    else
                    {
                        // posicao.Quantidade (atualização)
                        posicao.AtualizarQuantidade(quantidadeTotal);
                    }

                    ctx.Inventario.Update(posicao);
                    ctx.SaveChanges();

                    string mensagemLog = $"Entrada ID {entradaId}: Produto '{produto.nomeProduto}' (ID: {produtoId}) - " +
                                        $"Quantidade: {quantidadeRecebida} unidades - " +
                                        $"Fornecedor ID: {fornecedorId} - " +
                                        $"Posição: {posicao.NomePosicao} (ID: {inventarioId}) - " +
                                        $"Armazém ID: {posicao.ArmazemId}";
                    
                    RelatorioLogs.SalvarLog(ctx, mensagemLog);

                    return Results.Created("", new
                    {
                        entrada = novaEntrada,
                        inventarioAtualizado = posicao,
                        mensagem = $"Entrada registrada com sucesso! Produto {produto.nomeProduto} " +
                                  $"adicionado na posição {posicao.NomePosicao}. " +
                                  $"Quantidade na posição: {posicao.Quantidade}/{armazem.ProdutoPosicao}"
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0038", ex.Message).Message);
                }
            });

            app.MapDelete("/api/DeleteEntradaProduto={entradaId}&{produtoId}", 
                ([FromRoute] int entradaId, [FromRoute] int produtoId, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Caua, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Delete para remover registro de entrada.
                Args: entradaId(int), produtoId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Entrada não encontrada!")
                
                */

                EntradaProduto? resultado = ctx.EntradaProduto
                    .FirstOrDefault(x => x.EntradaId == entradaId && x.ProdutoId == produtoId);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0046").Message);
                }

                EntradaProduto.Deletar(ctx, entradaId, produtoId);

                return Results.Ok(resultado);
            });
        }
    }
}


/**
 * Autor: Vitor, Caua, Luan
 * Data de Criação: 18/10/2025
 * Descrição: Endpoints para controle de entrada de produtos no armazém
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

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

                return Results.NotFound("Nenhuma entrada encontrada!");
            });

            app.MapGet("/api/GetEntradaProdutoById={entradaId}", 
                ([FromRoute] int entradaId, [FromServices] AppDataContext ctx) =>
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
                    return Results.NotFound("Nenhuma entrada encontrada!");
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

                    // Validação: Fornecedor existe?
                    Fornecedor? fornecedor = ctx.Fornecedor.Find(fornecedorId);
                    if (fornecedor is null)
                    {
                        return Results.NotFound($"Fornecedor com ID {fornecedorId} não encontrado!");
                    }

                    // Validação: Produto existe?
                    Produto? produto = ctx.Produto.Find(produtoId);
                    if (produto is null)
                    {
                        return Results.NotFound($"Produto com ID {produtoId} não encontrado!");
                    }

                    // Validação: Posição do inventário existe?
                    Inventario? posicao = ctx.Inventario.Find(inventarioId);
                    if (posicao is null)
                    {
                        return Results.NotFound($"Posição de inventário com ID {inventarioId} não encontrada!");
                    }

                    // Validação: Quantidade válida?
                    if (quantidadeRecebida <= 0)
                    {
                        return Results.BadRequest("Quantidade recebida deve ser maior que zero!");
                    }

                    // Buscar o armazém para verificar capacidade
                    Armazem? armazem = ctx.Armazem.Find(posicao.ArmazemId);
                    if (armazem is null)
                    {
                        return Results.NotFound($"Armazém com ID {posicao.ArmazemId} não encontrado!");
                    }

                    // Validação PRINCIPAL: Posição já tem produto de outro ID?
                    if (posicao.ProdutoId.HasValue && posicao.ProdutoId.Value != produtoId)
                    {
                        return Results.BadRequest(
                            $"A posição {posicao.NomePosicao} já contém o produto ID {posicao.ProdutoId}! " +
                            $"Uma posição só pode ter produtos de um mesmo ID. " +
                            $"Escolha outra posição ou remova o produto atual.");
                    }

                    // Validação: Quantidade total não pode exceder o limite da posição
                    int quantidadeAtual = posicao.Quantidade;
                    int quantidadeTotal = quantidadeAtual + quantidadeRecebida;

                    if (quantidadeTotal > armazem.ProdutoPosicao)
                    {
                        return Results.BadRequest(
                            $"A posição {posicao.NomePosicao} suporta no máximo {armazem.ProdutoPosicao} unidades. " +
                            $"Quantidade atual: {quantidadeAtual}. " +
                            $"Tentando adicionar: {quantidadeRecebida}. " +
                            $"Total seria: {quantidadeTotal}. " +
                            $"Escolha outra posição ou reduza a quantidade.");
                    }

                    // Gerar ID único para a entrada
                    int entradaId = EntradaProduto.GerarEntradaId(ctx);

                    // Criar registro de entrada
                    EntradaProduto novaEntrada = EntradaProduto.Criar(entradaId, fornecedorId, produtoId, quantidadeRecebida);

                    ctx.EntradaProduto.Add(novaEntrada);

                    // Atualizar inventário
                    if (posicao.ProdutoId is null)
                    {
                        // Posição vazia - alocar produto
                        posicao.AlocarProduto(produtoId, quantidadeRecebida);
                    }
                    else
                    {
                        // Posição já tem o mesmo produto - adicionar quantidade
                        posicao.AtualizarQuantidade(quantidadeTotal);
                    }

                    ctx.Inventario.Update(posicao);
                    ctx.SaveChanges();

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
                    return Results.BadRequest($"Erro ao processar entrada: {ex.Message}");
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
                    return Results.NotFound("Entrada não encontrada!");
                }

                EntradaProduto.Deletar(ctx, entradaId, produtoId);

                return Results.Ok(resultado);
            });
        }
    }
}


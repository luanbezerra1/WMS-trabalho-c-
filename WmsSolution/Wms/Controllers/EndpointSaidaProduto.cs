/**
 * Autor: Vitor, Cauã, Luan
 * Data de Criação: 18/10/2025
 * Descrição: Endpoints para controle de saída de produtos do armazém
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointSaidaProduto
    {
        public static void MapEndpointsSaidaProduto(this WebApplication app)
        {
            /*
            
            Autor: Vitor, Cauã, Luan
            Data de Criação: 18/10/2025
            Descrição: Endpoints para controle de saída de produtos.
            Args: ctx(AppDataContext)
            
            */

            app.MapGet("/api/GetSaidaProduto", ([FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Cauã, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Get para listar todas as saídas de produtos.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.SaidaProduto.ToList()) ou Results.NotFound("Nenhuma saída encontrada!")
                
                */

                if (ctx.SaidaProduto.Any())
                {
                    return Results.Ok(ctx.SaidaProduto.ToList());
                }

                return Results.NotFound("Nenhuma saída encontrada!");
            });

            app.MapGet("/api/GetSaidaProdutoById/{saidaId}",
                ([FromRoute] int saidaId, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Cauã, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Get para buscar saídas pelo ID da saída.
                Args: saidaId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhuma saída encontrada!")
                
                */

                var resultado = ctx.SaidaProduto
                    .Where(x => x.SaidaId == saidaId)
                    .ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound("Nenhuma saída encontrada!");
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostSaidaProduto", ([FromBody] System.Text.Json.JsonElement dados, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Cauã, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Post para registrar saída de produto e atualizar inventário.
                Args: dados(JsonElement), ctx(AppDataContext)
                Return: Results.Created ou Results.BadRequest/NotFound
                
                Dados esperados:
                {
                    "clienteId": int,
                    "produtoId": int,
                    "quantidadeRetirada": int,
                    "inventarioId": int (posição de onde o produto será retirado)
                }
                
                */

                try
                {
                    int clienteId = dados.GetProperty("clienteId").GetInt32();
                    int produtoId = dados.GetProperty("produtoId").GetInt32();
                    int quantidadeRetirada = dados.GetProperty("quantidadeRetirada").GetInt32();
                    int inventarioId = dados.GetProperty("inventarioId").GetInt32();

                    // Validação: Cliente existe?
                    Cliente? cliente = ctx.Cliente.Find(clienteId);
                    if (cliente is null)
                    {
                        return Results.NotFound($"Cliente com ID {clienteId} não encontrado!");
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
                    if (quantidadeRetirada <= 0)
                    {
                        return Results.BadRequest("Quantidade retirada deve ser maior que zero!");
                    }

                    // Verificação: produto e quantidade disponíveis na posição
                    if (posicao.ProdutoId != produtoId)
                    {
                        return Results.BadRequest($"A posição {posicao.NomePosicao} não contém o produto ID {produtoId}!");
                    }

                    if (posicao.Quantidade < quantidadeRetirada)
                    {
                        return Results.BadRequest($"Quantidade insuficiente! Na posição há apenas {posicao.Quantidade} unidades.");
                    }

                    // Gerar ID único para a saída
                    int saidaId = SaidaProduto.GerarSaidaId(ctx);

                    // Criar registro de saída
                    SaidaProduto novaSaida = SaidaProduto.Criar(saidaId, clienteId, produtoId, quantidadeRetirada);

                    ctx.SaidaProduto.Add(novaSaida);

                    // Atualizar inventário (reduzir quantidade)
                    posicao.Quantidade -= quantidadeRetirada;

                    // Se quantidade zerar, limpa a posição
                    if (posicao.Quantidade <= 0)
                    {
                        posicao.ProdutoId = null;
                        posicao.Quantidade = 0;
                    }

                    ctx.Inventario.Update(posicao);
                    ctx.SaveChanges();

                    // 🔎 Log da SAÍDA de produto
                    string mensagemLog =
                        $"Saída ID {saidaId}: Produto '{produto.nomeProduto}' (ID: {produtoId}) - " +
                        $"Quantidade retirada: {quantidadeRetirada} unidades - " +
                        $"Cliente ID: {clienteId} - " +
                        $"Posição: {posicao.NomePosicao} (ID: {inventarioId}) - " +
                        $"Armazém ID: {posicao.ArmazemId}";

                    RelatorioLogs.SalvarLog(ctx, mensagemLog);
                    
                    return Results.Created("", new
                    {
                        saida = novaSaida,
                        inventarioAtualizado = posicao,
                        mensagem = $"Saída registrada com sucesso! Produto {produto.nomeProduto} retirado da posição {posicao.NomePosicao}. Quantidade restante: {posicao.Quantidade}"
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Erro ao processar saída: {ex.Message}");
                }
            });

            app.MapDelete("/api/DeleteSaidaProduto/{saidaId}/{produtoId}",
                ([FromRoute] int saidaId, [FromRoute] int produtoId, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Vitor, Cauã, Luan
                Data de Criação: 18/10/2025
                Descrição: Endpoint Delete para remover registro de saída.
                Args: saidaId(int), produtoId(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Saída não encontrada!")
                
                */

                SaidaProduto? resultado = ctx.SaidaProduto
                    .FirstOrDefault(x => x.SaidaId == saidaId && x.ProdutoId == produtoId);

                if (resultado is null)
                {
                    return Results.NotFound("Saída não encontrada!");
                }

                SaidaProduto.Deletar(ctx, saidaId, produtoId);

                return Results.Ok(resultado);
            });
        }
    }
}

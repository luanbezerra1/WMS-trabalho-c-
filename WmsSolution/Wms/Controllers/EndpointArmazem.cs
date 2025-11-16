/**
 * Autor: Caua
 * Data de Criação: 15/10/2025
 * Descrição: Endpoints para CRUD de Armazém
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;
using Wms.Enums;

namespace Wms.Controllers
{
    public static class EndpointArmazem
    {
        public static void MapEndpointsArmazem(this WebApplication app)
        {
            /*

            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Endpoints para CRUD de Armazém.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetArmazem", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para listar todos os armazéns.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Armazem.ToList()) ou Results.NotFound("Nenhum armazém encontrado!")

                */

                if (ctx.Armazem.Any())
                {
                    return Results.Ok(ctx.Armazem.ToList());
                }

                return Results.NotFound(EnumTipoException.ThrowException("MSG0055").Message);
            });

            app.MapGet("/api/GetArmazemById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar um armazém por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Armazém não encontrado!")

                */

                Armazem? resultado = ctx.Armazem.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0056").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetArmazemByStatus={status}", ([FromRoute] string status, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar armazéns por status.
                Args: status(string), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Nenhum armazém encontrado com esse status!")

                */

                var resultado = ctx.Armazem.Where(x => x.status == status).ToList();

                if (!resultado.Any())
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0057").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostArmazem", ([FromBody] Armazem armazem, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Post para cadastrar um armazém.
                Args: armazem(Armazem), ctx(AppDataContext)
                Return: Results.Created("", novoArmazem) ou Results.Conflict("Esse armazém já existe!")

                */

                if (armazem.Id != 0)
                {
                    Armazem? resultado = ctx.Armazem.FirstOrDefault(x => x.Id == armazem.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict(EnumTipoException.ThrowException("MSG0058").Message);
                    }
                }

                if (string.IsNullOrEmpty(armazem.nomeArmazem))
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0059").Message);
                }

                if (armazem.EnderecoId > 0)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(armazem.EnderecoId);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0019", armazem.EnderecoId).Message);
                    }
                }

                if (armazem.Posicoes <= 0)
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0060").Message);
                }

                if (armazem.ProdutoPosicao <= 0)
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0061").Message);
                }
                
                Armazem novoArmazem = Armazem.Criar(armazem.nomeArmazem, armazem.status, armazem.Posicoes, armazem.ProdutoPosicao, armazem.EnderecoId);
                novoArmazem.Id = Armazem.GerarId(ctx);
                
                ctx.Armazem.Add(novoArmazem);
                ctx.SaveChanges();

                // Criar posicoes (inventario)
                for (int i = 1; i <= novoArmazem.Posicoes; i++)
                {
                    string nomePosicao = $"P{novoArmazem.Id:D3}{i:D3}";
                    Inventario posicao = Inventario.CriarPosicaoVazia(novoArmazem.Id, nomePosicao);
                    ctx.Inventario.Add(posicao);
                }

                ctx.SaveChanges();

                return Results.Created("", novoArmazem);
            });

            app.MapPut("/api/PutArmazem={id}", ([FromRoute] int id, [FromBody] Armazem armazemAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para alterar um armazém.
                Args: id(int), armazemAlterado(Armazem), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Armazém não encontrado!")

                */

                Armazem? resultado = ctx.Armazem.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0056").Message);
                }

                if (string.IsNullOrEmpty(armazemAlterado.nomeArmazem))
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0059").Message);
                }

                if (armazemAlterado.EnderecoId > 0)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(armazemAlterado.EnderecoId);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0019", armazemAlterado.EnderecoId).Message);
                    }
                }

                if (armazemAlterado.Posicoes <= 0)
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0060").Message);
                }

                if (armazemAlterado.ProdutoPosicao <= 0)
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0061").Message);
                }
                
                resultado.Alterar(armazemAlterado.nomeArmazem, armazemAlterado.status, armazemAlterado.Posicoes, armazemAlterado.ProdutoPosicao, armazemAlterado.EnderecoId);

                ctx.Armazem.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });

            app.MapDelete("/api/DeleteArmazem={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Caua
                Data de Criação: 15/10/2025
                Descrição: Endpoint Delete para deletar um armazém.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Armazém não encontrado!")

                */

                Armazem? resultado = ctx.Armazem.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0056").Message);
                }
                
                Armazem.Deletar(ctx, id);

                return Results.Ok(resultado);
            });
        }
    }
}

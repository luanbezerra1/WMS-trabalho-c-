/**
 * Autor: Vitor
 * Data de Criação: 15/10/2025
 * Descrição: Endpoints para CRUD de Endereço
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointEndereco
    {
        public static void MapEndpointsEndereco(this WebApplication app)
        {
            /*

            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Endpoints para CRUD de Endereço.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetEndereco", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para listar todos os endereços.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Endereco.ToList()) ou Results.NotFound("Nenhum endereço encontrado!")

                */

                if (ctx.Endereco.Any())
                {
                    return Results.Ok(ctx.Endereco.ToList());
                }

                return Results.NotFound("Nenhum endereço encontrado!");
            });

            app.MapGet("/api/GetEnderecoById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar um endereço por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Endereço não encontrado!")

                */

                Endereco? resultado = ctx.Endereco.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound("Endereço não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostEndereco", ([FromBody] Endereco endereco, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Post para cadastrar um endereço.
                Args: endereco(Endereco), ctx(AppDataContext)
                Return: Results.Created("", novoEndereco) ou Results.Conflict("Esse endereço já existe!")

                */

                if (endereco.Id != 0)
                {
                    Endereco? resultado = ctx.Endereco.FirstOrDefault(x => x.Id == endereco.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict("Esse endereço já existe!");
                    }
                }
                
                Endereco novoEndereco = Endereco.Criar(endereco.Rua, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.Cidade, endereco.Estado, endereco.Cep);
                
                ctx.Endereco.Add(novoEndereco);
                ctx.SaveChanges();

                return Results.Created("", novoEndereco);
            });

            app.MapDelete("/api/DeleteEndereco={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Delete para deletar um endereço.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Endereço não encontrado!")

                */

                Endereco? resultado = ctx.Endereco.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Endereço não encontrado!");
                }
                
                Endereco.Deletar(ctx, id);

                return Results.Ok(resultado);
            });

            app.MapPut("/api/PutEndereco={id}", ([FromRoute] int id, [FromBody] Endereco enderecoAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para alterar um endereço.
                Args: id(int), enderecoAlterado(Endereco), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Endereço não encontrado!")

                */

                Endereco? resultado = ctx.Endereco.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Endereço não encontrado!");
                }
                
                resultado.Alterar(enderecoAlterado.Rua, enderecoAlterado.Numero, enderecoAlterado.Complemento, enderecoAlterado.Bairro, enderecoAlterado.Cidade, enderecoAlterado.Estado, enderecoAlterado.Cep);

                ctx.Endereco.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });
        }
    }
}


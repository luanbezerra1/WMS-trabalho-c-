/**
 * Autor: Vitor
 * Data de Criação: 12/10/2025
 * Descrição: Endpoints para CRUD de Usuário
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;
using Wms.Enums;

namespace Wms.Controllers
{
    public static class EndpointUsuario
    {
        public static void MapEndpointsUsuario(this WebApplication app)
        {
            /*

            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Endpoints para CRUD de Usuário.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetUsuario", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Get para listar todos os usuários.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Usuario.ToList()) ou Results.NotFound("Nenhum usuário encontrado!")

                */

                if (ctx.Usuario.Any())
                {
                    return Results.Ok(ctx.Usuario.ToList());
                }

                return Results.NotFound(EnumTipoException.ThrowException("MSG0001").Message);
            });

            app.MapGet("/api/GetUsuarioById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Get para buscar um usuário por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Usuário não encontrado!")

                */

                Usuario? resultado = ctx.Usuario.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0002").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetUsuarioByLogin={login}", ([FromRoute] string login, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Get para buscar um usuário por login.
                Args: login(string), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Usuário não encontrado!")

                */

                Usuario? resultado = ctx.Usuario.FirstOrDefault(x => x.login == login);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0002").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostUsuario", ([FromBody] Usuario usuario, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Post para cadastrar um usuário.
                Args: usuario(Usuario), ctx(AppDataContext)
                Return: Results.Created("", novoUsuario) ou Results.Conflict("Esse usuário já existe!")

                */

                if (usuario.Id != 0)
                {
                    Usuario? resultado = ctx.Usuario.FirstOrDefault(x => x.Id == usuario.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict(EnumTipoException.ThrowException("MSG0003").Message);
                    }
                }

                Usuario? loginExistente = ctx.Usuario.FirstOrDefault(x => x.login == usuario.login);
                if (loginExistente is not null)
                {
                    return Results.Conflict(EnumTipoException.ThrowException("MSG0004").Message);
                }
                
                Usuario novoUsuario = Usuario.Criar(usuario.nome, usuario.login, usuario.senha, usuario.cargo);
                novoUsuario.Id = Usuario.GerarId(ctx);
                
                ctx.Usuario.Add(novoUsuario);
                ctx.SaveChanges();

                return Results.Created("", novoUsuario);
            });

            app.MapDelete("/api/DeleteUsuario={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Delete para deletar um usuário.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Usuário não encontrado!")

                */

                Usuario? resultado = ctx.Usuario.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0002").Message);
                }
                
                Usuario.Deletar(ctx, id);

                return Results.Ok(resultado);
            });

            app.MapPut("/api/PutUsuario={id}", ([FromRoute] int id, [FromBody] Usuario usuarioAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 12/10/2025
                Descrição: Endpoint Put para alterar um usuário.
                Args: id(int), usuarioAlterado(Usuario), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Usuário não encontrado!")

                */

                Usuario? resultado = ctx.Usuario.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Usuário não encontrado!");
                }

                Usuario? loginExistente = ctx.Usuario.FirstOrDefault(x => x.login == usuarioAlterado.login && x.Id != id);
                if (loginExistente is not null)
                {
                    return Results.Conflict(EnumTipoException.ThrowException("MSG0004").Message);
                }
                
                resultado.Alterar(usuarioAlterado.nome, usuarioAlterado.login, usuarioAlterado.senha, usuarioAlterado.cargo);

                ctx.Usuario.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });
        }
    }
}



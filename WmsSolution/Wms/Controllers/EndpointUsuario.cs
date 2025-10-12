/**
 * Autor: Vitor
 * Data de Criação: 12/10/2025
 * Descrição: Endpoints para CRUD de Usuário
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

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

                return Results.NotFound("Nenhum usuário encontrado!");
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

                Usuario? resultado = ctx.Usuario.FirstOrDefault(x => x.id == id);

                if (resultado is null)
                {
                    return Results.NotFound("Usuário não encontrado!");
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
                    return Results.NotFound("Usuário não encontrado!");
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

                if (usuario.id != 0)
                {
                    Usuario? resultado = ctx.Usuario.FirstOrDefault(x => x.id == usuario.id);

                    if (resultado is not null)
                    {
                        return Results.Conflict("Esse usuário já existe!");
                    }
                }

                // Valida se o login já existe
                Usuario? loginExistente = ctx.Usuario.FirstOrDefault(x => x.login == usuario.login);
                if (loginExistente is not null)
                {
                    return Results.Conflict("Esse login já está em uso!");
                }
                
                Usuario novoUsuario = Usuario.Criar(usuario.nome, usuario.login, usuario.senha, usuario.cargo);
                novoUsuario.id = Usuario.GerarId(ctx);
                
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
                    return Results.NotFound("Usuário não encontrado!");
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

                // Valida se o login já existe (exceto para o próprio usuário)
                Usuario? loginExistente = ctx.Usuario.FirstOrDefault(x => x.login == usuarioAlterado.login && x.id != id);
                if (loginExistente is not null)
                {
                    return Results.Conflict("Esse login já está em uso!");
                }
                
                resultado.Alterar(usuarioAlterado.nome, usuarioAlterado.login, usuarioAlterado.senha, usuarioAlterado.cargo);

                ctx.Usuario.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });
        }
    }
}



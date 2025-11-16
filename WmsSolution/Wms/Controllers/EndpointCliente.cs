/**
 * Autor: Vitor
 * Data de Criação: 15/10/2025
 * Descrição: Endpoints para CRUD de Cliente
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;
using Wms.Enums;

namespace Wms.Controllers
{
    public static class EndpointCliente
    {
        public static void MapEndpointsCliente(this WebApplication app)
        {
            /*

            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Endpoints para CRUD de Cliente.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetCliente", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para listar todos os clientes.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Cliente.ToList()) ou Results.NotFound("Nenhum cliente encontrado!")

                */

                if (ctx.Cliente.Any())
                {
                    return Results.Ok(ctx.Cliente.ToList());
                }

                return Results.NotFound(EnumTipoException.ThrowException("MSG0020").Message);
            });

            app.MapGet("/api/GetClienteById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar um cliente por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Cliente não encontrado!")

                */

                Cliente? resultado = ctx.Cliente.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0021").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetClienteByCpf={cpf}", ([FromRoute] string cpf, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Get para buscar um cliente por CPF.
                Args: cpf(string), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Cliente não encontrado!")

                */

                Cliente? resultado = ctx.Cliente.FirstOrDefault(x => x.Cpf == cpf);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0021").Message);
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostCliente", ([FromBody] Cliente cliente, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Post para cadastrar um cliente.
                Args: cliente(Cliente), ctx(AppDataContext)
                Return: Results.Created("", novoCliente) ou Results.Conflict("Esse cliente já existe!")

                */

                if (cliente.Id != 0)
                {
                    Cliente? resultado = ctx.Cliente.FirstOrDefault(x => x.Id == cliente.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict(EnumTipoException.ThrowException("MSG0050").Message);
                    }
                }

                if (string.IsNullOrEmpty(cliente.Cpf))
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0051").Message);
                }

                Cliente? cpfExistente = ctx.Cliente.FirstOrDefault(x => x.Cpf == cliente.Cpf);
                if (cpfExistente is not null)
                {
                    return Results.Conflict(EnumTipoException.ThrowException("MSG0022").Message);
                }

                if (cliente.EnderecoId > 0)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(cliente.EnderecoId);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0019", cliente.EnderecoId).Message);
                    }
                }
                
                Cliente novoCliente = Cliente.Criar(cliente.Nome, cliente.Email, cliente.Telefone, cliente.Cpf, cliente.EnderecoId);
                novoCliente.Id = Cliente.GerarId(ctx);
                
                ctx.Cliente.Add(novoCliente);
                ctx.SaveChanges();

                return Results.Created("", novoCliente);
            });

            app.MapPut("/api/PutCliente={id}", ([FromRoute] int id, [FromBody] Cliente clienteAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Put para alterar um cliente.
                Args: id(int), clienteAlterado(Cliente), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Cliente não encontrado!")

                */

                Cliente? resultado = ctx.Cliente.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound(EnumTipoException.ThrowException("MSG0021").Message);
                }

                if (string.IsNullOrEmpty(clienteAlterado.Cpf))
                {
                    return Results.BadRequest(EnumTipoException.ThrowException("MSG0051").Message);
                }

                Cliente? cpfExistente = ctx.Cliente.FirstOrDefault(x => x.Cpf == clienteAlterado.Cpf && x.Id != id);
                if (cpfExistente is not null)
                {
                    return Results.Conflict(EnumTipoException.ThrowException("MSG0022").Message);
                }

                if (clienteAlterado.EnderecoId > 0)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(clienteAlterado.EnderecoId);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound(EnumTipoException.ThrowException("MSG0019", clienteAlterado.EnderecoId).Message);
                    }
                }
                
                resultado.Alterar(clienteAlterado.Nome, clienteAlterado.Email, clienteAlterado.Telefone, clienteAlterado.Cpf, clienteAlterado.EnderecoId);

                ctx.Cliente.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });

            app.MapDelete("/api/DeleteCliente={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 15/10/2025
                Descrição: Endpoint Delete para deletar um cliente.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Cliente não encontrado!")

                */

                Cliente? resultado = ctx.Cliente.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Cliente não encontrado!");
                }
                
                Cliente.Deletar(ctx, id);

                return Results.Ok(resultado);
            });
        }
    }
}


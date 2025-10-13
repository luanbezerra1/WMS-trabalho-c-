/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Endpoints para CRUD de Fornecedor
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointFornecedor
    {
        public static void MapEndpointsFornecedor(this WebApplication app)
        {
            /*

            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Endpoints para CRUD de Fornecedor.
            Args: ctx(AppDataContext)

            */

            app.MapGet("/api/GetFornecedor", ([FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para listar todos os fornecedores.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.Fornecedor.ToList()) ou Results.NotFound("Nenhum fornecedor encontrado!")

                */

                if (ctx.Fornecedor.Any())
                {
                    return Results.Ok(ctx.Fornecedor.ToList());
                }

                return Results.NotFound("Nenhum fornecedor encontrado!");
            });

            app.MapGet("/api/GetFornecedorById={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para buscar um fornecedor por ID.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Fornecedor não encontrado!")

                */

                Fornecedor? resultado = ctx.Fornecedor.FirstOrDefault(x => x.Id == id);

                if (resultado is null)
                {
                    return Results.NotFound("Fornecedor não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapGet("/api/GetFornecedorByCnpj={cnpj}", ([FromRoute] string cnpj, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Get para buscar um fornecedor por CNPJ.
                Args: cnpj(string), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Fornecedor não encontrado!")

                */

                Fornecedor? resultado = ctx.Fornecedor.FirstOrDefault(x => x.cnpj == cnpj);

                if (resultado is null)
                {
                    return Results.NotFound("Fornecedor não encontrado!");
                }

                return Results.Ok(resultado);
            });

            app.MapPost("/api/PostFornecedor", ([FromBody] Fornecedor fornecedor, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Post para cadastrar um fornecedor.
                Args: fornecedor(Fornecedor), ctx(AppDataContext)
                Return: Results.Created("", novoFornecedor) ou Results.Conflict("Esse fornecedor já existe!")

                */

                if (fornecedor.Id != 0)
                {
                    Fornecedor? resultado = ctx.Fornecedor.FirstOrDefault(x => x.Id == fornecedor.Id);

                    if (resultado is not null)
                    {
                        return Results.Conflict("Esse fornecedor já existe!");
                    }
                }

                // Valida se o CNPJ foi informado
                if (string.IsNullOrEmpty(fornecedor.cnpj))
                {
                    return Results.BadRequest("Fornecedor deve ter CNPJ informado!");
                }

                // Valida se CNPJ já existe
                Fornecedor? cnpjExistente = ctx.Fornecedor.FirstOrDefault(x => x.cnpj == fornecedor.cnpj);
                if (cnpjExistente is not null)
                {
                    return Results.Conflict("Esse CNPJ já está cadastrado!");
                }

                // Valida se o endereço existe (caso tenha sido informado)
                if (fornecedor.enderecoId.HasValue)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(fornecedor.enderecoId.Value);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound($"Endereço com ID {fornecedor.enderecoId} não encontrado!");
                    }
                }
                
                Fornecedor novoFornecedor = Fornecedor.Criar(fornecedor.nome, fornecedor.email, fornecedor.telefone, fornecedor.cnpj, fornecedor.enderecoId);
                novoFornecedor.Id = Fornecedor.GerarId(ctx);
                
                ctx.Fornecedor.Add(novoFornecedor);
                ctx.SaveChanges();

                return Results.Created("", novoFornecedor);
            });

            app.MapDelete("/api/DeleteFornecedor={id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Delete para deletar um fornecedor.
                Args: id(int), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Fornecedor não encontrado!")

                */

                Fornecedor? resultado = ctx.Fornecedor.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Fornecedor não encontrado!");
                }
                
                Fornecedor.Deletar(ctx, id);

                return Results.Ok(resultado);
            });

            app.MapPut("/api/PutFornecedor={id}", ([FromRoute] int id, [FromBody] Fornecedor fornecedorAlterado, [FromServices] AppDataContext ctx) =>
            {
                /*

                Autor: Vitor
                Data de Criação: 13/10/2025
                Descrição: Endpoint Put para alterar um fornecedor.
                Args: id(int), fornecedorAlterado(Fornecedor), ctx(AppDataContext)
                Return: Results.Ok(resultado) ou Results.NotFound("Fornecedor não encontrado!")

                */

                Fornecedor? resultado = ctx.Fornecedor.Find(id);

                if (resultado is null)
                {
                    return Results.NotFound("Fornecedor não encontrado!");
                }

                // Valida se o CNPJ foi informado
                if (string.IsNullOrEmpty(fornecedorAlterado.cnpj))
                {
                    return Results.BadRequest("Fornecedor deve ter CNPJ informado!");
                }

                // Valida se CNPJ já existe (exceto para o próprio fornecedor)
                Fornecedor? cnpjExistente = ctx.Fornecedor.FirstOrDefault(x => x.cnpj == fornecedorAlterado.cnpj && x.Id != id);
                if (cnpjExistente is not null)
                {
                    return Results.Conflict("Esse CNPJ já está cadastrado!");
                }

                // Valida se o endereço existe (caso tenha sido informado)
                if (fornecedorAlterado.enderecoId.HasValue)
                {
                    Endereco? enderecoExistente = ctx.Endereco.Find(fornecedorAlterado.enderecoId.Value);
                    if (enderecoExistente is null)
                    {
                        return Results.NotFound($"Endereço com ID {fornecedorAlterado.enderecoId} não encontrado!");
                    }
                }
                
                resultado.Alterar(fornecedorAlterado.nome, fornecedorAlterado.email, fornecedorAlterado.telefone, fornecedorAlterado.cnpj, fornecedorAlterado.enderecoId);

                ctx.Fornecedor.Update(resultado);
                ctx.SaveChanges();
                
                return Results.Ok(resultado);
            });
        }
    }
}


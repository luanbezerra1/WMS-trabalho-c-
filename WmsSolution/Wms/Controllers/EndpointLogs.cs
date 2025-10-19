/**
 * Autor: Luan
 * Data de Criação: 19/10/2025
 * Descrição: Controller para geração de relatórios e logs do sistema
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    public static class EndpointLogs
    {
        public static void MapEndpointsLogs(this WebApplication app)
        {
            /*
            
            Autor: Luan
            Data de Criação: 19/10/2025
            Descrição: Endpoints para consulta de logs de entrada de produtos.
            Args: ctx(AppDataContext)
            
            */

            app.MapGet("/api/GetLogs", ([FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Luan
                Data de Criação: 19/10/2025
                Descrição: Endpoint Get para listar todos os logs.
                Args: ctx(AppDataContext)
                Return: Results.Ok(ctx.RelatorioLogs.ToList()) ou Results.NotFound("Nenhum log encontrado!")
                
                */

                if (ctx.RelatorioLogs.Any())
                {
                    return Results.Ok(ctx.RelatorioLogs.OrderByDescending(l => l.DataHora).ToList());
                }

                return Results.NotFound("Nenhum log encontrado!");
            });

            app.MapGet("/api/GetLogById={logId}", ([FromRoute] int logId, [FromServices] AppDataContext ctx) =>
            {
                /*
                
                Autor: Luan
                Data de Criação: 19/10/2025
                Descrição: Endpoint Get para buscar log específico pelo ID.
                Args: logId(int), ctx(AppDataContext)
                Return: Results.Ok(log) ou Results.NotFound("Log não encontrado!")
                
                */

                RelatorioLogs? log = ctx.RelatorioLogs.Find(logId);

                if (log is null)
                {
                    return Results.NotFound("Log não encontrado!");
                }

                return Results.Ok(log);
            });
        }
    }
}

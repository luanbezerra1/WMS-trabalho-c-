/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Controller para geração de relatórios e logs do sistema
**/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Wms.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class RelatorioLogsController : ControllerBase
    {
        private readonly ILogger<RelatorioLogsController> _logger;

        public RelatorioLogsController(ILogger<RelatorioLogsController> logger)
        {
            _logger = logger;
        }

        // Log de ENTRADA (fornecedor + produto)
        [HttpPost("entrada")]
        public IActionResult RegistrarEntrada(
            [FromQuery] int fornecedorId,
            [FromQuery] int produtoId,
            [FromQuery] int quantidade,
            [FromQuery] string? observacao)
        {
            var agora = DateTime.Now;

            // imprime no console
            _logger.LogInformation(
                "LOG ENTRADA | FornecedorId: {FornecedorId} | ProdutoId: {ProdutoId} | Qtd: {Qtd} | Obs: {Obs} | Data: {DataHora}",
                fornecedorId, produtoId, quantidade, observacao ?? "-", agora
            );

        
            return Ok(new
            {
                mensagem = "Log de ENTRADA registrado",
                fornecedorId,
                produtoId,
                quantidade,
                observacao,
                horario = agora.ToString("dd/MM/yyyy HH:mm:ss")
            });
        }

        
    }
}
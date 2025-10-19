/**
 * Autor: Luan
 * Data de Criação: 19/10/2025
 * Descrição: Modelo de dados para registro de logs de entrada de produtos
**/

using System.ComponentModel.DataAnnotations;

namespace Wms.Models;

public class RelatorioLogs
{
    [Key]
    public int LogId { get; set; }

    public string Mensagem { get; set; } = string.Empty;
    
    public DateTime DataHora { get; set; }

    public RelatorioLogs() { }

    public static RelatorioLogs Criar(string mensagem)
    {
        return new RelatorioLogs
        {
            Mensagem = mensagem,
            DataHora = DateTime.Now
        };
    }

    public static void SalvarLog(AppDataContext ctx, string mensagem)
    {
        RelatorioLogs novoLog = Criar(mensagem);
        ctx.RelatorioLogs.Add(novoLog);
        ctx.SaveChanges();
    }
}


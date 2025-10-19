/**
 * Autor: Vitor, Cauã, Luan
 * Data de Criação: 18/10/2025
 * Descrição: Classe para gerenciamento de saída de produtos do armazém
**/

namespace Wms.Models
{
    public class SaidaProduto
    {
        public int SaidaId { get; set; }

        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public int ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int QuantidadeRetirada { get; set; }

        public DateTime DataSaida { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        public static int GerarSaidaId(AppDataContext ctx)
        {
            /*
             * Autor: Cauã
             * Data de Criação : 18/10/2025
             * Descrição: Gera um ID único para a saída de produto.
             * Args: ctx(AppDataContext)
             * Return: int
            */

            if (!ctx.SaidaProduto.Any())
                return 1;

            return ctx.SaidaProduto.Max(s => s.SaidaId) + 1;
        }  

        public static SaidaProduto Criar(int saidaId, int clienteId, int produtoId, int quantidadeRetirada)
        {
            /*
             * Autor: Cauã
             * Data de Criação : 18/10/2025
             * Descrição: Cria um objeto de saída de produto.
             * Args: saidaId(int), clienteId(int), produtoId(int), quantidadeRetirada(int)
             * Return: SaidaProduto
            */

            return new SaidaProduto
            {
                SaidaId = saidaId,
                ClienteId = clienteId,
                ProdutoId = produtoId,
                QuantidadeRetirada = quantidadeRetirada,
                DataSaida = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };
        }

        public static void Deletar(AppDataContext ctx, int saidaId, int produtoId)
        {
            /*
             * Autor: Cauã
             * Data de Criação : 18/10/2025
             * Descrição: Remove uma saída de produto do banco.
             * Args: ctx(AppDataContext), saidaId(int), produtoId(int)
             * Return: None
            */

            SaidaProduto? saida = ctx.SaidaProduto.FirstOrDefault(s => s.SaidaId == saidaId && s.ProdutoId == produtoId);

            if (saida is not null)
            {
                ctx.SaidaProduto.Remove(saida);
                ctx.SaveChanges();
            }
        }
    }
}

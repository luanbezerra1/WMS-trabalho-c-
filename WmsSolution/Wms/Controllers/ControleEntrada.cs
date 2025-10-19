/**
 * Autor: Vitor
 * Data de Criação: 15/10/2025
 * Descrição: Modelo para gerenciamento de entrada de produtos no armazém
**/

namespace Wms.Models
{
    public class EntradaProduto
    {
        public int EntradaId { get; set; }

        public int FornecedorId { get; set; }

        public Fornecedor? Fornecedor { get; set; }

        public int ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int QuantidadeRecebida { get; set; }
        
        public DateTime DataEntrada { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        public static int GerarEntradaId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 18/10/2025
            Descrição: Método responsável por gerar um ID para a entrada.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */

            if (!ctx.EntradaProduto.Any())
            {
                return 1;
            }
            return ctx.EntradaProduto.Max(e => e.EntradaId) + 1;
        }

        public static EntradaProduto Criar(int entradaId, int fornecedorId, int produtoId, int quantidadeRecebida)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 18/10/2025
            Descrição: Método responsável por criar uma entrada de produto.
            Args: entradaId(int), fornecedorId(int), produtoId(int), quantidadeRecebida(int)
            Return: EntradaProduto(EntradaProduto)
            
            */

            return new EntradaProduto
            {
                EntradaId = entradaId,
                FornecedorId = fornecedorId,
                ProdutoId = produtoId,
                QuantidadeRecebida = quantidadeRecebida,
                DataEntrada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };
        }

        public static void Deletar(AppDataContext ctx, int entradaId, int produtoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 18/10/2025
            Descrição: Método responsável por deletar uma entrada de produto.
            Args: ctx(AppDataContext), entradaId(int), produtoId(int)
            Return: None
            
            */

            EntradaProduto? entrada = ctx.EntradaProduto.FirstOrDefault(e => e.EntradaId == entradaId && e.ProdutoId == produtoId);
            
            if (entrada is not null)
            {
                ctx.EntradaProduto.Remove(entrada);
                ctx.SaveChanges();
            }
        }
    }
}
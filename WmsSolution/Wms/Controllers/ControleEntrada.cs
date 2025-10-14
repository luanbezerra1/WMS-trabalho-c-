/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Controller para gerenciamento de entrada de produtos no armazém
**/

namespace Wms.Models
{
    public class EntradaProduto
    {
        public int EntradaId { get; set; }
        
        // FK 
        public int FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        // FK
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public int QuantidadeRecebida { get; set; }

        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;
    }
}
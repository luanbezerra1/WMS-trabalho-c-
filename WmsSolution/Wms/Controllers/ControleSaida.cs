/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Controller para gerenciamento de saída de produtos do armazém
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

        public DateTime DataSaida { get; set; } = DateTime.UtcNow;  
        
    }
}
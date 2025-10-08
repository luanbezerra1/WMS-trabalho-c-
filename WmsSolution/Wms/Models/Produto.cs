/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Produto
**/

namespace Wms.Models

{
    public class Produto
    {
        public int skuId { get; set; }
        public string nomeProduto { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public int lote { get; set; }
        public Fornecedor? fornecedor { get; set; }
        public double preco { get; set; }
    }
}

/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Produto
**/

namespace Wms.Models

{
    public class Produto
    {
        public int Id { get; set; }
         public string Sku { get; set; } = "";
        public string nomeProduto { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public int lote { get; set; }
        public Fornecedor? fornecedor { get; set; }
        public double preco { get; set; }
         public Categorias Categoria { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}

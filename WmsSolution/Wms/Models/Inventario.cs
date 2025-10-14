/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Modelo de dados para controle de inventário do armazém
**/

namespace Wms.Models
{
   public class Inventario
{
    public int ArmazemId { get; set; }
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }
    public DateTime ultimaMovimentacao { get; set; } = DateTime.UtcNow;
}
}
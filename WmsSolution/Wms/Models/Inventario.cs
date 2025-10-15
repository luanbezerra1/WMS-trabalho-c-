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
    public Armazem? Armazem { get; set; }

    public int PosicaoID {get;set;}

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public int Quantidade { get; set; }
    public DateTime UltimaMovimentacao { get; set; } = DateTime.UtcNow;
}
}
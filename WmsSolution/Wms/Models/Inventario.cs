/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Modelo de dados para controle de inventário do armazém
**/

namespace Wms.Models
{
    public class Inventario
    {
        public int armazemId { get; set; }
        public int posicaoId { get; set; } // ex: "P101"
        public int produtoId { get; set; }
        public int quantidade { get; set; }
        public DateTime ultimaMovimentacao { get; set; } = DateTime.UtcNow;
    }
}
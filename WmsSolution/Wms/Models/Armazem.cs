/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Armazém
**/

namespace Wms.Models
{
    public class Armazem
    {
        public int Id { get; set; }
        public string nomeArmazem { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty; // Ativo (1) ou Inativo (0)
        public int posicoes { get; set; } // Quantidade de posições do armazém
        public int produtoPosicao { get; set; } // Quantidade de produtos que cada posição pode armazenar
        public int capacidade { get; set; } // Capacidade total de posições do armazém (posicoes * produtoPosicao)
        public int? enderecoId { get; set; }
    }
}

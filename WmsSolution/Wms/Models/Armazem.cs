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
        public Endereco? endereco { get; set; }
        public string status { get; set; } = string.Empty; // Ativo (1) ou Inativo (0)
        public int capacidade { get; set; } // Capacidade total de posições do armazém
    }
}

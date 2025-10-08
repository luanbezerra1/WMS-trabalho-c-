/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Armazém
**/

namespace Wms.Models
{
    public class Armazem
    {
        public int id { get; set; }
        public string nomeArmazem { get; set; } = string.Empty;
        public Endereco? endereco { get; set; }
        public string status { get; set; } = string.Empty; // Ativo ou Inativo
        public int capacidade { get; set; } // Capacidade total de posiçõesdo armazém
    }
}

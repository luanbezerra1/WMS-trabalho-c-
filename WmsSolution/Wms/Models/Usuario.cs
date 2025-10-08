/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Usuário
**/

namespace Wms.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string login { get; set; } = string.Empty;
        public string senha { get; set; } = string.Empty;
        public string cargo { get; set; } = string.Empty;
    }
}

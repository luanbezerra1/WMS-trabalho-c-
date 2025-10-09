/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Pessoa
**/

namespace Wms.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public Endereco? endereco { get; set; }
    }
}

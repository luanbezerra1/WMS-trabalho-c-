/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Endereço
**/

namespace Wms.Models
{
    public class Endereco
    {
        public int id { get; set; }
        public string rua { get; set; } = string.Empty;
        public string numero { get; set; } = string.Empty;
        public string complemento { get; set; } = string.Empty;
        public string bairro { get; set; } = string.Empty;
        public string cidade { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string cep { get; set; } = string.Empty;
    }
}


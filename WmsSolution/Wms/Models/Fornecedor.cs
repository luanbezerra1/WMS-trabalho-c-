/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Fornecedor
**/

namespace Wms.Models
{
    public class Fornecedor : Pessoa
    {
        public string cnpj { get; set; } = string.Empty;
    }
}

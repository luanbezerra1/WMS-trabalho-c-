/**
 * Autor: Vitor
 * Data de Criação: 12/10/2025
 * Descrição: Modelo de dados para representação de Cadastro (Cliente/Fornecedor)
**/

namespace Wms.Models
{
    public class Cadastro
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public string? cpf { get; set; }
        public string? cnpj { get; set; }
        public int tipo { get; set; } // 0 = Cliente (CPF), 1 = Fornecedor (CNPJ)
        public int? idEndereco { get; set; }
    }
}


/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Cliente
**/

namespace Wms.Models
{
    public class Cliente : Pessoa
    {
        public string cpf { get; set; } = string.Empty;
    }
}

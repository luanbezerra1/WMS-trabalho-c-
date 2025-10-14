/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Armazém
**/

namespace Wms.Models
{
    public class Armazem
    {
        public int Id { get; set; } // PK                 
        public string nomeArmazem { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;  // "Ativo"/"Inativo" (ou use bool)

        public int Posicoes { get; set; }                   // quantidade, não são as posições em si

        public int ProdutoPosicao { get; set; }

        public int Capacidade { get; set; } // posicoes * produtoPosicao
        public int? enderecoId { get; set; } // FK opcional para Endereco (se quiser)
    }
}

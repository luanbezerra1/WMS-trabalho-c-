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
        public string status { get; set; } = string.Empty;  
        public int Posicoes { get; set; }  
        public int ProdutoPosicao { get; set; }
        public int Capacidade { get; set; }          // posicoes * produtoPosicao
        public int EnderecoId { get; set; }  // FK 
        public Endereco? Endereco { get; set; } 


        // Relação 1:N entre Armazem e Inventario
        // Um Armazem pode conter vários Inventarios (1:N)
        // Essa propriedade permite acessar todos os inventários dentro de um armazém

        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
    }



/**
 * Autor: Vitor
 * Data de Criação: 15/10/2025
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

        public int Capacidade { get; set; } // posicoes * produtoPosicao

        public int EnderecoId { get; set; }  

        public Endereco? Endereco { get; set; } 

        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

        public static int GerarId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por gerar um ID para o armazém.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */
    
            if (!ctx.Armazem.Any())
            {
                return 1;
            }
            return ctx.Armazem.Max(a => a.Id) + 1;
        }

        public static Armazem Criar(string nomeArmazem, string status, int posicoes, int produtoPosicao, int enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por criar um armazém.
            Args: nomeArmazem(string), status(string), posicoes(int), produtoPosicao(int), enderecoId(int)
            Return: Armazem(Armazem)
            
            */
        
            return new Armazem
            {
                nomeArmazem = nomeArmazem,
                status = status,
                Posicoes = posicoes,
                ProdutoPosicao = produtoPosicao,
                Capacidade = posicoes * produtoPosicao,
                EnderecoId = enderecoId
            };
        }

        public void Alterar(string nomeArmazem, string status, int posicoes, int produtoPosicao, int enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por alterar um armazém.
            Args: nomeArmazem(string), status(string), posicoes(int), produtoPosicao(int), enderecoId(int)
            Return: None
            
            */

            this.nomeArmazem = nomeArmazem;
            this.status = status;
            this.Posicoes = posicoes;
            this.ProdutoPosicao = produtoPosicao;
            this.Capacidade = posicoes * produtoPosicao;
            this.EnderecoId = enderecoId;
        }

        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por deletar um armazém.
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Armazem? armazem = ctx.Armazem.Find(id);
            
            if (armazem is not null)
            {
                var inventarios = ctx.Inventario.Where(i => i.ArmazemId == id).ToList();
                ctx.Inventario.RemoveRange(inventarios);
                
                ctx.Armazem.Remove(armazem);
                ctx.SaveChanges();
            }
        }
    }
}



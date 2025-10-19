/**
 * Autor: Caua
 * Data de Criação: 15/10/2025
 * Descrição: Modelo de dados para controle de inventário do armazém
**/

namespace Wms.Models
{
    public class Inventario
    {
        public int Id { get; set; }

        public int ArmazemId { get; set; }

        public Armazem? Armazem { get; set; }

        public string NomePosicao { get; set; } = string.Empty;

        public int? ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int Quantidade { get; set; }
        
        public DateTime UltimaMovimentacao { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        public static Inventario CriarPosicaoVazia(int armazemId, string nomePosicao)
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por criar uma posição vazia no inventário.
            Args: armazemId(int), nomePosicao(string)
            Return: Inventario(Inventario)
            
            */
        
            return new Inventario
            {
                ArmazemId = armazemId,
                NomePosicao = nomePosicao,
                ProdutoId = null,
                Quantidade = 0,
                UltimaMovimentacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };
        }

        public static Inventario Criar(int armazemId, string nomePosicao, int produtoId, int quantidade)
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por criar um inventário com produto.
            Args: armazemId(int), nomePosicao(string), produtoId(int), quantidade(int)
            Return: Inventario(Inventario)
            
            */
        
            return new Inventario
            {
                ArmazemId = armazemId,
                NomePosicao = nomePosicao,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                UltimaMovimentacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };
        }

        public void AlocarProduto(int produtoId, int quantidade)
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por alocar um produto em uma posição.
            Args: produtoId(int), quantidade(int)
            Return: None
            
            */

            this.ProdutoId = produtoId;
            this.Quantidade = quantidade;
            this.UltimaMovimentacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public void AtualizarQuantidade(int quantidade)
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por atualizar quantidade de um inventário.
            Args: quantidade(int)
            Return: None
            
            */

            this.Quantidade = quantidade;
            this.UltimaMovimentacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public void RemoverProduto()
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por remover produto de uma posição (deixa vazia).
            Args: None
            Return: None
            
            */

            this.ProdutoId = null;
            this.Quantidade = 0;
            this.UltimaMovimentacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Caua
            Data de Criação: 15/10/2025
            Descrição: Metodo responsavel por deletar um inventário (posição).
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Inventario? inventario = ctx.Inventario.Find(id);
            if (inventario is not null)
            {
                ctx.Inventario.Remove(inventario);
                ctx.SaveChanges();
            }
        }
    }
}
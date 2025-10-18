/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Produto
**/

namespace Wms.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string nomeProduto { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public int lote { get; set; }
        public int fornecedorId { get; set; }
        public double preco { get; set; }
        public Categorias Categoria { get; set; }
        public DateTime CriadoEm { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));


        // Relação 1:N entre Produto e Inventario
        // Um Produto pode estar presente em vários registros de Inventario 
        // Essa coleção permite acessar todos os inventários onde o produto aparece.
        
        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
        
        public static int GerarId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por gerar um ID para o produto.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */
    
            if (!ctx.Produto.Any())
            {
                return 1;
            }
            return ctx.Produto.Max(p => p.Id) + 1;
        }

        
        public static Produto Criar(string nomeProduto, string descricao, int lote, int fornecedorId, double preco, Categorias categoria)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por criar um produto.
            Args: nomeProduto(string), descricao(string), lote(int), fornecedorId(int), preco(double), categoria(Categorias)
            Return: Produto(Produto)
            
            */
        
            return new Produto
            {
                nomeProduto = nomeProduto,
                descricao = descricao,
                lote = lote,
                fornecedorId = fornecedorId,
                preco = preco,
                Categoria = categoria,
                CriadoEm = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };
        }

        
        public void Alterar(string nomeProduto, string descricao, int lote, int fornecedorId, double preco, Categorias categoria)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por alterar um produto.
            Args: nomeProduto(string), descricao(string), lote(int), fornecedorId(int), preco(double), categoria(Categorias)
            Return: None
            
            */

            this.nomeProduto = nomeProduto;
            this.descricao = descricao;
            this.lote = lote;
            this.fornecedorId = fornecedorId;
            this.preco = preco;
            this.Categoria = categoria;
        }

        
        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por deletar um produto.
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Produto? produto = ctx.Produto.Find(id);
            if (produto is not null)
            {
                ctx.Produto.Remove(produto);
                ctx.SaveChanges();
            }
        }
    }
}

/**

 * Autor: Vitor Baraçal Gimarães
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Endereço

**/

namespace Wms.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        
        public ICollection<Cliente>? Clientes { get; set; } //1:N

        public static int GerarId(AppDataContext ctx)
        {
            /*

            Autor: Vitor Baraçal Gimarães
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por gerar um ID para o endereço.
            Args: ctx(AppDataContext)
            Return: id(int)

            */
    
            if (!ctx.Endereco.Any())
            {
                return 1;
            }
            return ctx.Endereco.Max(e => e.Id) + 1;
        }

        
        public static Endereco Criar(string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Cep)
        {
            /*

            Autor: Vitor Baraçal Gimarães
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por criar um endereço.
            Args: rua(string), numero(string), complemento(string), bairro(string), cidade(string), estado(string), cep(string)
            Return: Endereco(Endereco)

            */
        
            return new Endereco
            {

                Rua = Rua,  
                Numero = Numero,
                Complemento = Complemento,
                Bairro = Bairro,
                Cidade = Cidade,
                Estado = Estado,
                Cep = Cep
            };
        }

        public void Alterar(string Rua, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string Cep)
        {
            /*

            Autor: Vitor Baraçal Gimarães
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por alterar um endereço.
            Args: rua(string), numero(string), complemento(string), bairro(string), cidade(string), estado(string), cep(string)
            Return: None

            */

            this.Rua = Rua;
            this.Numero = Numero;
            this.Complemento = Complemento;
            this.Bairro = Bairro;
            this.Cidade = Cidade;
            this.Estado = Estado;
            this.Cep = Cep;
        }

        
        public static void Deletar(AppDataContext ctx, int id)
        {
            /*

            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por deletar um endereço.
            Args: ctx(AppDataContext), id(int)
            Return: None

            */

            Endereco? endereco = ctx.Endereco.Find(id);
            if (endereco is not null)
            {
                ctx.Endereco.Remove(endereco);
                ctx.SaveChanges();
            }
        }
    }
}


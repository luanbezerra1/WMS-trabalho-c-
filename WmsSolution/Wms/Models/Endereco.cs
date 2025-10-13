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
        public string rua { get; set; } = string.Empty;
        public string numero { get; set; } = string.Empty;
        public string complemento { get; set; } = string.Empty;
        public string bairro { get; set; } = string.Empty;
        public string cidade { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string cep { get; set; } = string.Empty;

        
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

        
        public static Endereco Criar(string rua, string numero, string complemento, string bairro, string cidade, string estado, string cep)
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

                rua = rua,  
                numero = numero,
                complemento = complemento,
                bairro = bairro,
                cidade = cidade,
                estado = estado,
                cep = cep
            };
        }

        public void Alterar(string rua, string numero, string complemento, string bairro, string cidade, string estado, string cep)
        {
            /*

            Autor: Vitor Baraçal Gimarães
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por alterar um endereço.
            Args: rua(string), numero(string), complemento(string), bairro(string), cidade(string), estado(string), cep(string)
            Return: None

            */

            this.rua = rua;
            this.numero = numero;
            this.complemento = complemento;
            this.bairro = bairro;
            this.cidade = cidade;
            this.estado = estado;
            this.cep = cep;
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


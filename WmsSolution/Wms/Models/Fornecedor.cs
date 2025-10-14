/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Modelo de dados para representação de Fornecedor
**/

namespace Wms.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public string cnpj { get; set; } = string.Empty;

        public int EnderecoId { get; set; }           // FK obrigatória
        public Endereco? Endereco { get; set; }       // navegação (opcional)

        public static int GerarId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por gerar um ID para o fornecedor.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */

            if (!ctx.Fornecedor.Any())
            {
                return 1;
            }
            return ctx.Fornecedor.Max(f => f.Id) + 1;
        }

        public static Fornecedor Criar(string nome, string email, string telefone, string cnpj, int enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por criar um fornecedor.
            Args: nome(string), email(string), telefone(string), cnpj(string), enderecoId(int)
            Return: Fornecedor(Fornecedor)
            
            */

            return new Fornecedor
            {
                nome = nome,
                email = email,
                telefone = telefone,
                cnpj = cnpj,
                EnderecoId = enderecoId   
            };
        }

        public void Alterar(string nome, string email, string telefone, string cnpj, int enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por alterar um fornecedor.
            Args: nome(string), email(string), telefone(string), cnpj(string), enderecoId(int)
            Return: None
            
            */

            this.nome = nome;
            this.email = email;
            this.telefone = telefone;
            this.cnpj = cnpj;
            this.EnderecoId = enderecoId;   
        }

        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por deletar um fornecedor.
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Fornecedor? fornecedor = ctx.Fornecedor.Find(id);
            if (fornecedor is not null)
            {
                ctx.Fornecedor.Remove(fornecedor);
                ctx.SaveChanges();
            }
        }
    }
}
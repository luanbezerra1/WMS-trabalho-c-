/**
 * Autor: Vitor
 * Data de Criação: 13/10/2025
 * Descrição: Modelo de dados para representação de Cliente
**/

namespace Wms.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public string cpf { get; set; } = string.Empty;
        public int? enderecoId { get; set; }

        
        public static int GerarId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por gerar um ID para o cliente.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */
    
            if (!ctx.Cliente.Any())
            {
                return 1;
            }
            return ctx.Cliente.Max(c => c.Id) + 1;
        }

        
        public static Cliente Criar(string nome, string email, string telefone, string cpf, int? enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por criar um cliente.
            Args: nome(string), email(string), telefone(string), cpf(string), enderecoId(int?)
            Return: Cliente(Cliente)
            
            */
        
            return new Cliente
            {
                nome = nome,
                email = email,
                telefone = telefone,
                cpf = cpf,
                enderecoId = enderecoId
            };
        }

        
        public void Alterar(string nome, string email, string telefone, string cpf, int? enderecoId)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por alterar um cliente.
            Args: nome(string), email(string), telefone(string), cpf(string), enderecoId(int?)
            Return: None
            
            */

            this.nome = nome;
            this.email = email;
            this.telefone = telefone;
            this.cpf = cpf;
            this.enderecoId = enderecoId;
        }

        
        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 13/10/2025
            Descrição: Metodo responsavel por deletar um cliente.
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Cliente? cliente = ctx.Cliente.Find(id);
            if (cliente is not null)
            {
                ctx.Cliente.Remove(cliente);
                ctx.SaveChanges();
            }
        }
    }
}


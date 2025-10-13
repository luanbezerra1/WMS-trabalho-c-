/**
 * Autor: Vitor
 * Data de Criação: 08/10/2025
 * Descrição: Modelo de dados para representação de Usuário
**/

namespace Wms.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string login { get; set; } = string.Empty;
        public string senha { get; set; } = string.Empty;
        public string cargo { get; set; } = string.Empty;

        
        public static int GerarId(AppDataContext ctx)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por gerar um ID para o usuário.
            Args: ctx(AppDataContext)
            Return: id(int)
            
            */
    
            if (!ctx.Usuario.Any())
            {
                return 1;
            }
            return ctx.Usuario.Max(u => u.Id) + 1;
        }

        
        public static Usuario Criar(string nome, string login, string senha, string cargo)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por criar um usuário.
            Args: nome(string), login(string), senha(string), cargo(string)
            Return: Usuario(Usuario)
            
            */
        
            return new Usuario
            {
                nome = nome,
                login = login,
                senha = senha,
                cargo = cargo
            };
        }

        
        public void Alterar(string nome, string login, string senha, string cargo)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por alterar um usuário.
            Args: nome(string), login(string), senha(string), cargo(string)
            Return: None
            
            */

            this.nome = nome;
            this.login = login;
            this.senha = senha;
            this.cargo = cargo;
        }

        
        public static void Deletar(AppDataContext ctx, int id)
        {
            /*
            
            Autor: Vitor
            Data de Criação: 12/10/2025
            Descrição: Metodo responsavel por deletar um usuário.
            Args: ctx(AppDataContext), id(int)
            Return: None
            
            */

            Usuario? usuario = ctx.Usuario.Find(id);
            if (usuario is not null)
            {
                ctx.Usuario.Remove(usuario);
                ctx.SaveChanges();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Wms.Models;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }


    // Tabelas do Banco de Dados
    public DbSet<Produto> Produto { get; set; }
    public DbSet<Armazem> Armazem { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Endereco> Endereco { get; set; }
    public DbSet<Cadastro> Cadastro { get; set; }

}
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
    public DbSet<Inventario> Inventario { get; set; }

    protected override void OnModelCreating(ModelBuilder b)
{
    base.OnModelCreating(b);

    // PK composta (Armazem + Posição + Produto)
    b.Entity<Inventario>()
     .HasKey(i => new { i.ArmazemId, i.PosicaoCodigo, i.ProdutoId });

    b.Entity<Inventario>()
     .HasIndex(i => new { i.ArmazemId, i.PosicaoCodigo });

    // Validações básicas
    b.Entity<Inventario>()
     .Property(i => i.PosicaoCodigo)
     .HasMaxLength(20)               
     .IsRequired();
}
}
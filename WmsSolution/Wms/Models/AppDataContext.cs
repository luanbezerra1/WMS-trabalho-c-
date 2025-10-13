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
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Fornecedor> Fornecedor { get; set; }
    public DbSet<Inventario> Inventario { get; set; }

    protected override void OnModelCreating(ModelBuilder b)
{
    base.OnModelCreating(b);

    // PK composta (Armazem + Posição + Produto)
    b.Entity<Inventario>()
     .HasKey(i => new { i.armazemId, i.posicaoId, i.produtoId });

    b.Entity<Inventario>()
     .HasIndex(i => new { i.armazemId, i.posicaoId });

    // Validações básicas
    b.Entity<Inventario>()
     .Property(i => i.quantidade)
     .IsRequired();
     
    b.Entity<Inventario>()
     .Property(i => i.ultimaMovimentacao)
     .IsRequired();
}
}
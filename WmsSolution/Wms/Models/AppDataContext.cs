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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // INVENTARIO — PK composta (Armazem + Produto)
        
        modelBuilder.Entity<Inventario>()
            .HasKey(i => new { i.ArmazemId, i.ProdutoId });

        modelBuilder.Entity<Inventario>()
            .HasOne<Armazem>()
            .WithMany()
            .HasForeignKey(i => i.ArmazemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventario>()
            .Property(i => i.Quantidade)
            .IsRequired();

        modelBuilder.Entity<Inventario>()
            .Property(i => i.ultimaMovimentacao)
            .IsRequired();

    // ------------------------------------------------------
        // CLIENTE -> ENDERECO (FK obrigatória)

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Endereco)
            .WithMany(e => e.Clientes)
            .HasForeignKey(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);

        // CPF único
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Cpf)
            .IsUnique();

    // ------------------------------------------------------
        // FORNECEDOR -> ENDERECO 

        modelBuilder.Entity<Fornecedor>()
            .HasOne(f => f.Endereco)
            .WithMany()
            .HasForeignKey(f => f.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);

        // CNPJ único 
        modelBuilder.Entity<Fornecedor>()
            .HasIndex(f => f.cnpj)
            .IsUnique();
            
    // ------------------------------------------------------

        // CONTROLE_ENTRADA — PK composta + relacionamentos

        modelBuilder.Entity<EntradaProduto>()
            .HasKey(e => new { e.EntradaId, e.ProdutoId });

        modelBuilder.Entity<EntradaProduto>()
            .HasOne(e => e.Fornecedor)
            .WithMany()
            .HasForeignKey(e => e.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EntradaProduto>()
            .HasOne(e => e.Produto)
            .WithMany()
            .HasForeignKey(e => e.ProdutoId)luan
            .OnDelete(DeleteBehavior.Restrict);


        // CONTROLE_SAÍDA — PK composta + relacionamentos
        
        modelBuilder.Entity<SaidaProduto>()
            .HasKey(s => new { s.SaidaId, s.ProdutoId });

        modelBuilder.Entity<SaidaProduto>()
            .HasOne(s => s.Cliente)
            .WithMany()
            .HasForeignKey(s => s.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SaidaProduto>()
            .HasOne(s => s.Produto)
            .WithMany()
            .HasForeignKey(s => s.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
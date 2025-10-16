/**
 * Autor: Luan
 * Data de Criação: 15/10/2025
 * Descrição: Modelo de dados para representação de AppDataContext
**/

using Microsoft.EntityFrameworkCore;

namespace Wms.Models;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

    public DbSet<Produto> Produto { get; set; }
    public DbSet<Armazem> Armazem { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Endereco> Endereco { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Fornecedor> Fornecedor { get; set; }
    public DbSet<Inventario> Inventario { get; set; }
    public DbSet<EntradaProduto> EntradaProduto {get ; set;}   
    public DbSet<SaidaProduto> SaidaProduto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // INVENTARIO - Ignorar navegações
        modelBuilder.Entity<Inventario>()
            .Ignore(i => i.Armazem)
            .Ignore(i => i.Produto);

        // ARMAZEM - Ignorar navegações
        modelBuilder.Entity<Armazem>()
            .Ignore(a => a.Inventarios)
            .Ignore(a => a.Endereco);

        // PRODUTO - Ignorar navegações
        modelBuilder.Entity<Produto>()
            .Ignore(p => p.Inventarios);

        // CLIENTE - Ignorar navegações
        modelBuilder.Entity<Cliente>()
            .Ignore(c => c.Endereco);

        // FORNECEDOR - Ignorar navegações
        modelBuilder.Entity<Fornecedor>()
            .Ignore(f => f.Endereco);

        // ENDERECO - Ignorar navegações
        modelBuilder.Entity<Endereco>()
            .Ignore(e => e.Clientes);

        // ENTRADA_PRODUTO - Ignorar navegações
        modelBuilder.Entity<EntradaProduto>()
            .Ignore(e => e.Fornecedor)
            .Ignore(e => e.Produto);

        // SAIDA_PRODUTO - Ignorar navegações
        modelBuilder.Entity<SaidaProduto>()
            .Ignore(s => s.Cliente)
            .Ignore(s => s.Produto);

        // INVENTARIO - Índice único: nome da posição deve ser único por armazém
        modelBuilder.Entity<Inventario>()
            .HasIndex(i => new { i.ArmazemId, i.NomePosicao })
            .IsUnique();

        // CLIENTE - CPF único
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Cpf)
            .IsUnique();

        // FORNECEDOR - CNPJ único
        modelBuilder.Entity<Fornecedor>()
            .HasIndex(f => f.cnpj)
            .IsUnique();

        // CONTROLE_ENTRADA - PK composta
        modelBuilder.Entity<EntradaProduto>()
            .HasKey(e => new { e.EntradaId, e.ProdutoId });

        // CONTROLE_SAÍDA - PK composta
        modelBuilder.Entity<SaidaProduto>()
            .HasKey(s => new { s.SaidaId, s.ProdutoId });
    }
}
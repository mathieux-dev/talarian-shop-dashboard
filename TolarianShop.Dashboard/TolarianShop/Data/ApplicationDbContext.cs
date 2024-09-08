using Microsoft.EntityFrameworkCore;
using TolarianShop.Models;

namespace TolarianShop.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<NotaFiscal> NotaFiscal { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotaFiscal>()
            .Property(nf => nf.Valor)
            .HasColumnType("decimal(18,2)");
    }
}


using FiscalManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiscalManager.Infrastructure.Configurations
{
    public class FiscalDbContext : DbContext
    {
        public FiscalDbContext(DbContextOptions<FiscalDbContext> options) : base(options) { }
        
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Precisão decimal para valores monetários
            modelBuilder.Entity<Invoice>(entity => {
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(255);
            });
        }
    }
}

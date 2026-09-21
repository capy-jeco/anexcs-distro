using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central;

public class CentralDbContext : DbContext, ICentralUnitOfWork
{
    public CentralDbContext(
        DbContextOptions<CentralDbContext> options)
        : base(options) { }
    
    public DbSet<Domain.Entities.Central.Tenant> Tenants => Set<Domain.Entities.Central.Tenant>();
    public DbSet<Domain.Entities.Central.TenantDomain> TenantDomains => Set<Domain.Entities.Central.TenantDomain>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Central.Tenant>(b =>
        {
            b.HasKey(t => t.Id);
            b.Property(t => t.Data).HasColumnType("jsonb");
            b.Property(t => t.DatabaseName).HasMaxLength(63); // Postgres identifier limit
            b.Property(t => t.ServerKey).HasMaxLength(100);
            b.HasMany(t => t.Domains)
                .WithOne(d => d.Tenant)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Domain.Entities.Central.TenantDomain>(b =>
        {
            b.HasKey(d => d.Id);
            b.HasIndex(d => d.Domain)
                .IsUnique();
        });
    }
}
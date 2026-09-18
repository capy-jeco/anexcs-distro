using Anexcs.Distro.Application.Abstractions.Persistence;
using Anexcs.Distro.Domain.Entities;
using Anexcs.Distro.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central;

public class CentralDbContext : DbContext, IUnitOfWork
{
    public CentralDbContext(
        DbContextOptions<CentralDbContext> options)
        : base(options) { }
    
    public DbSet<Domain.Entities.Tenant> Tenants => Set<Domain.Entities.Tenant>();
    public DbSet<TenantDomain> Domains => Set<TenantDomain>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Tenant>(b =>
        {
            b.HasKey(t => t.Id);
            b.Property(t => t.Data).HasColumnType("jsonb");
            b.HasMany(t => t.Domains)
                .WithOne(d => d.Tenant)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TenantDomain>(b =>
        {
            b.HasKey(d => d.Id);
            b.HasIndex(d => d.Domain)
                .IsUnique();
        });
    }
}
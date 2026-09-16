using Microsoft.EntityFrameworkCore;

namespace Anexcs.Distro.Infrastructure.Persistence.Central;

public class CentralDbContext : DbContext
{
    public CentralDbContext(
        DbContextOptions<CentralDbContext> options)
        : base(options)
    {
    }
}
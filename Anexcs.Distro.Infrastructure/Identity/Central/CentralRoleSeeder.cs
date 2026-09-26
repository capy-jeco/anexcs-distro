using Anexcs.Distro.Application.Common.Constants;
using Microsoft.AspNetCore.Identity;

namespace Anexcs.Distro.Infrastructure.Identity.Central;

public static class CentralRoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in CentralRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
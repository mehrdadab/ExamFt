using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamFT.Identity.Data;

internal static class RoleSeeder
{
    private static readonly string[] Roles = ["Examiner", "Examinee"];

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExamFT.Identity.Data;

// Used by EF Core CLI tools (dotnet ef migrations add / update) when Aspire is not running.
internal sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__identitydb")
            ?? "Host=localhost;Port=5432;Database=identitydb;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new ApplicationDbContext(options);
    }
}

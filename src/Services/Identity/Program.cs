using ExamFT.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<ApplicationDbContext>("identitydb", configureDbContextOptions: options =>
{
    options.UseSnakeCaseNamingConvention();
    options.UseNpgsql(npgsqlOptions =>
    {
        npgsqlOptions.MigrationsHistoryTable("__efmigrationshistory", "public");
    });

});

builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        options.Password.RequiredLength         = 8;
        options.Password.RequireDigit           = true;
        options.Password.RequireUppercase       = true;
        options.Password.RequireNonAlphanumeric = true;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(15);
        options.Lockout.AllowedForNewUsers      = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

await RoleSeeder.SeedAsync(app.Services);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlyFlags.Core;
using OnlyFlags.Core.Domain;
using OnlyFlags.Persistence.Sqlite;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddOnlyFlags()
    .AddOnlyFlagsSqliteDbContext(builder.Configuration);

using var host = builder.Build();

await MigrateDatabaseAsync(host);

await AddNewFeatureFlags(host);

await MigrateDatabaseAsync(host);

await ShowFeatureFlags(host);

await host.RunAsync();

async Task MigrateDatabaseAsync(IHost appHost)
{
    using var scope = appHost.Services.CreateScope();
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception exception)
    {
        Console.WriteLine($"❌ Database Migration Error: {exception}");
    }

    Console.WriteLine("✅ DB Migration Completed.");
}

async Task ShowFeatureFlags(IHost appHost)
{
    using var scope = appHost.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    var flags = await dbContext.FeatureFlags.Select(x => x.Name).ToArrayAsync();

    if (flags.Length == 0)
    {
        Console.WriteLine("There are no feature flags.");
        return;
    }
    
    Console.WriteLine("Feature Flags:");
    foreach (var flag in flags)
    {
        Console.WriteLine(flag);
    }
}

async Task AddNewFeatureFlags(IHost appHost)
{
    using var scope = appHost.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    
    for (var i = 0; i < 5; i++)
    {
        dbContext.FeatureFlags.Add(new FeatureFlag
        {
            Name = $"Feature-{Path.GetRandomFileName()}",
            Description = $"Description{i}",
            Enabled = true,
            CreatedAt = DateTime.UtcNow
        });
    }
    
    await dbContext.SaveChangesAsync();
}
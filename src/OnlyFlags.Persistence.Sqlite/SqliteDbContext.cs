using Microsoft.Extensions.DependencyInjection;
using OnlyFlags.Core.Persistence;

namespace OnlyFlags.Persistence.Sqlite;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options) : AppDbContext<SqliteDbContext>(options)
{
    protected override void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDbContext).Assembly);
    }
}

public static class ServicesRegistrationExtensions
{
    public static IServiceCollection AddSqliteDbContext(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        // todo: register db context factory ??
        services
            .AddDbContext<IAppDbContext, SqliteDbContext>(options =>
                options.UseSqlite(connectionString,
                    x => x.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));

        return services;
    }
}
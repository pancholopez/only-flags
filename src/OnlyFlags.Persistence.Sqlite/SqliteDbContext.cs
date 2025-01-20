namespace OnlyFlags.Persistence.Sqlite;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options): AppDbContext<SqliteDbContext>(options)
{
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDbContext).Assembly);
    }
}

/// <summary>
/// A factory for creating instances of <see cref="SqliteDbContext"/> at design time.
/// </summary>
/// <remarks>
/// The purpose of this class is to be able to create migrations directly from the class library
/// </remarks>
public class SqliteDbContextFactory : IDesignTimeDbContextFactory<SqliteDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="SqliteDbContext"/>.
    /// </summary>
    /// <param name="args">The arguments used to configure the context. If a connection string is not provided, an in-memory database will be used.</param>
    /// <returns>A new instance of <see cref="SqliteDbContext"/>.</returns>
    public SqliteDbContext CreateDbContext(string[] args)
    {
        var connectionString = args.FirstOrDefault() ?? "Data Source=:memory:";
        var options = new DbContextOptionsBuilder<SqliteDbContext>()
            .UseSqlite(connectionString, 
                x=>x.MigrationsAssembly(typeof(SqliteDbContextFactory).Assembly.FullName))
            .Options;

        return new SqliteDbContext(options);
    }
}
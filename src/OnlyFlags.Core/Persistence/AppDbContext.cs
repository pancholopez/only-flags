using Microsoft.EntityFrameworkCore;
using OnlyFlags.Core.Domain;

namespace OnlyFlags.Core.Persistence;

public abstract class AppDbContext<T>(DbContextOptions<T> options) : DbContext(options) where T : DbContext
{
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<FeatureFlagAudit> FeatureFlagAudits => Set<FeatureFlagAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureModel(modelBuilder);
    }
    
    protected abstract void ConfigureModel(ModelBuilder modelBuilder);
}
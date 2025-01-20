using Microsoft.EntityFrameworkCore;
using OnlyFlags.Core.Domain;

namespace OnlyFlags.Core.Persistence;

public abstract class AppDbContext<T>(DbContextOptions<T> options) : DbContext(options) where T : DbContext
{
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<FeatureFlagAudit> FeatureFlagAudits => Set<FeatureFlagAudit>();

    public new abstract void OnModelCreating(ModelBuilder modelBuilder);
}
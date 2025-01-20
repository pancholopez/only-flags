namespace OnlyFlags.Persistence.Sqlite.Configuration;

public class FeatureFlagAuditConfiguration : IEntityTypeConfiguration<FeatureFlagAudit>
{
    public void Configure(EntityTypeBuilder<FeatureFlagAudit> builder)
    {
        builder.ToTable("FeatureFlagAudit");
        builder.HasKey(a => a.Id);
        builder.Property(f => f.Id)
            .HasValueGenerator<TimeBaseGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(a => a.PreviousValue)
            .HasColumnType("TEXT");

        builder.Property(a => a.NewValue)
            .HasColumnType("TEXT");

        builder.Property(a => a.ChangedAt)
            .HasColumnType("TEXT")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relationships
        builder.HasOne(a => a.FeatureFlag)
            .WithMany(f => f.Audits)
            .HasForeignKey(a => a.FeatureFlagId);
    }
}
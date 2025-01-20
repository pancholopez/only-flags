namespace OnlyFlags.Persistence.Sqlite.Configuration;

public class FeatureFlagConfiguration : IEntityTypeConfiguration<FeatureFlag>
{
    public void Configure(EntityTypeBuilder<FeatureFlag> builder)
    {
        builder.ToTable("FeatureFlags");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .HasValueGenerator<TimeBaseGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Description)
            .HasMaxLength(250);

        builder.Property(f => f.Enabled)
            .IsRequired();

        builder.Property(f => f.StartDate)
            .HasColumnType("TEXT");

        builder.Property(f => f.EndDate)
            .HasColumnType("TEXT");

        builder.Property(f => f.CreatedAt)
            .HasColumnType("TEXT")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relationships
        builder.HasMany(f => f.Audits)
            .WithOne(a => a.FeatureFlag)
            .HasForeignKey(a => a.FeatureFlagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
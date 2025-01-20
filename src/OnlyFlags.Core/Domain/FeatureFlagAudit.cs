namespace OnlyFlags.Core.Domain;

public class FeatureFlagAudit : Entity<Guid>
{
    public Guid FeatureFlagId { get; set; }
    public Guid ChangedBy { get; set; }
    public string PreviousValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public FeatureFlag FeatureFlag { get; set; } = null!;
}
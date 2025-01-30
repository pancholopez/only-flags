using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnlyFlags.Core.Domain;
using OnlyFlags.Core.Persistence;

namespace OnlyFlags.Core.Application;

public interface IFeatureFlagProvider
{
    Task<bool> IsEnabledAsync<TFeature>() where TFeature : IFeature;
    Task<bool> IsEnabledAsync(string featureName);
}

public interface IFeatureFlagManager
{
    public Task UpdateAllFeatureFlagsAsync(bool enabled);
    public Task UpdateFeatureFlagAsync(Guid featureId, bool enabled);
}

public class FeatureFlagFacade(AppDbContext<DbContext> dbContext) : IFeatureFlagProvider, IFeatureFlagManager
{
    private readonly ConcurrentDictionary<string, bool> _cache = new();

    public async Task<bool> IsEnabledAsync<TFeature>() where TFeature : IFeature
    {
        var featureName = typeof(TFeature).Name;
        return await IsEnabledAsync(featureName);
    }

    public async Task<bool> IsEnabledAsync(string featureName)
    {
        if (_cache.TryGetValue(featureName, out var cachedValue))
        {
            return cachedValue;
        }

        var featureFlag = await dbContext.FeatureFlags
            .Where(f => f.Name == featureName)
            .FirstOrDefaultAsync();

        if (featureFlag == null)
            return false; // Default to disabled if flag doesn't exist

        var isActive = featureFlag.Enabled &&
                       (featureFlag.StartDate == null || featureFlag.StartDate <= DateTime.UtcNow) &&
                       (featureFlag.EndDate == null || featureFlag.EndDate >= DateTime.UtcNow);

        _cache[featureName] = isActive; // Cache result
        return isActive;
    }

    public async Task UpdateAllFeatureFlagsAsync(bool enabled)
    {
        foreach (var flag in dbContext.FeatureFlags)
        {
            flag.Enabled = enabled;
        }
        await dbContext.SaveChangesAsync();
        InvalidateAllCache();
    }

    public async Task UpdateFeatureFlagAsync(Guid featureId, bool enabled)
    {
        var featureFlag = await dbContext.FeatureFlags.FindAsync(featureId);
        if (featureFlag is null) return;

        featureFlag.Enabled = enabled;

        dbContext.FeatureFlagAudit.Add(new FeatureFlagAudit
        {
            FeatureFlagId = featureFlag.Id,
            ChangedBy = Guid.Empty,
            ChangedAt = DateTime.UtcNow,
            NewValue = System.Text.Json.JsonSerializer.Serialize(featureFlag)
        });
        
        await dbContext.SaveChangesAsync();

        // Clear cache for this feature
        InvalidateCache(featureFlag.Name);
    }

    private void InvalidateCache(string featureName) => _cache.TryRemove(featureName, out _);

    private void InvalidateAllCache() => _cache.Clear();
}
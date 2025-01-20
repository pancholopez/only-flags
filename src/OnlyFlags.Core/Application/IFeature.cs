using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnlyFlags.Core.Persistence;

namespace OnlyFlags.Core.Application;

public interface IFeature
{
}

public class DarkModeFeature : IFeature
{
}

public class BetaChatFeature : IFeature
{
}

public interface IFeatureFlagProvider
{
    Task<bool> IsEnabledAsync<TFeature>() where TFeature : IFeature;
    Task<bool> IsEnabledAsync(string featureName);
}

public class FeatureFlagProvider(IAppDbContext dbContext) : IFeatureFlagProvider
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

    // todo: move to feature flag manager interface
    public async Task UpdateFeatureFlagAsync(Guid featureId, bool enabled)
    {
        var featureFlag = await dbContext.FeatureFlags.FindAsync(featureId);
        if (featureFlag is null) return;

        featureFlag.Enabled = enabled;
        await dbContext.SaveChangesAsync();

        // Clear cache for this feature
        InvalidateCache(featureFlag.Name);
    }

    // todo: move to feature flag manager interface
    private void InvalidateCache(string featureName)
    {
        _cache.TryRemove(featureName, out _);
    }

    // todo: move to feature flag manager interface
    private void InvalidateAllCache()
    {
        _cache.Clear();
    }
}
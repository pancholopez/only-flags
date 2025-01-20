using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace OnlyFlags.Persistence.Sqlite.Configuration;

public class TimeBaseGuidValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry) => Guid.CreateVersion7();

    public override bool GeneratesTemporaryValues => false;
}
namespace OnlyFlags.Core.Shared;

/// <summary>
/// Represents an abstract base class for entities in the domain model.
/// Entities are objects with unique identities and may have properties and behavior associated with them.
/// </summary>
/// <typeparam name="T">The type of the identifier for the entity.</typeparam>
public abstract class Entity<T> : IEquatable<Entity<T>>
{
    /// <summary>
    /// Domain Entity Unique Identifier
    /// </summary>
    public T Id { get; } = default!;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Entity<T>)obj);
    }

    /// <inheritdoc />
    public bool Equals(Entity<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id!.Equals(other.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id!.GetHashCode();

    /// <summary>
    /// Two Domain entities are considered equal if their IDs are the same
    /// </summary>
    /// <param name="first">first entity</param>
    /// <param name="second">second entity</param>
    /// <returns>true if both ids are the same, otherwise, false</returns>
    public static bool operator ==(Entity<T>? first, Entity<T>? second)
        => first is not null && second is not null && first.Equals(second);

    /// <summary>
    /// Two Domain entities are considered NOT equal if their IDs are NOT the same
    /// </summary>
    /// <param name="first">first entity</param>
    /// <param name="second">second entity</param>
    /// <returns>true if both ids are NOT the same, otherwise, false</returns>
    public static bool operator !=(Entity<T>? first, Entity<T>? second) => !(first == second);
}
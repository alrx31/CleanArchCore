namespace Core.Abstractions.Data;

/// <summary>
/// Interface for entities with Guid primary key.
/// </summary>
public interface IGuidIdDbEntity : IDbEntity<Guid>;

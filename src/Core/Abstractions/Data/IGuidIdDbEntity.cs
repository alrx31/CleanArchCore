using System;

namespace Abstractions.Entity;

/// <summary>
/// Interface for entities with Guid primary key.
/// </summary>
public interface IGuidIdDbEntity : IDbEntity<Guid>;

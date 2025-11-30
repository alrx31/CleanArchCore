using System;

namespace Abstractions.Entity;

/// <summary>
/// Interface for entities with int primary key.
/// </summary>
public interface IIntIdDbEntity : IDbEntity<int>;
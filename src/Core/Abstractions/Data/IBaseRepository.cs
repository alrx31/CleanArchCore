using System.Linq.Expressions;

namespace Core.Abstractions.Data;

public interface IBaseRepository<T, TKey>
    where T : class, IDbEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    Task<List<TValue>> GetAsync<TValue> (
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T,TValue>>? selector = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default
    );

    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
}
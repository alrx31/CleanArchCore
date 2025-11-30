using System.Linq.Expressions;
using Core.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Data;

public abstract class BaseRepository<T, TKey>(DbContext context) : IBaseRepository<T, TKey>
    where T : class, IDbEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected DbContext Context { get; } = context;
    
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<T>().Where(filter ?? (_ => true));
        return query.CountAsync(cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<List<TValue>> GetAsync<TValue>(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, TValue>>? selector = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<T>()
        .Where(filter ?? (_ => true))
        .Select(selector ?? (e => (TValue)(object)e))
        .Skip(skip ?? 0)
        .Take(take ?? int.MaxValue);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync(keyValues: [id], cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
}

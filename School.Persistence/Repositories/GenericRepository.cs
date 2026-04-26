using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;

namespace School.Persistence.Repositories;

public class GenericRepository<T>(AppDbContext db) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _set = db.Set<T>();

    // ──────────────────────────────────────────
    // READ – single
    // ──────────────────────────────────────────

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _set.FindAsync([id], ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                               CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(predicate, ct);

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                                CancellationToken ct = default)
        => await _set.SingleOrDefaultAsync(predicate, ct);

    // ──────────────────────────────────────────
    // READ – collection
    // ──────────────────────────────────────────

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _set.ToListAsync(ct);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                                   CancellationToken ct = default)
        => await _set.Where(predicate).ToListAsync(ct);

    public async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>>?   predicate,
        Expression<Func<T, object>>? orderBy,
        bool                          ascending = true,
        int                           skip      = 0,
        int                           take      = int.MaxValue,
        CancellationToken             ct        = default)
    {
        IQueryable<T> query = _set;

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        if (skip > 0)  query = query.Skip(skip);
        if (take < int.MaxValue) query = query.Take(take);

        return await query.ToListAsync(ct);
    }

    // ──────────────────────────────────────────
    // READ – paged
    // ──────────────────────────────────────────

    public async Task<IReadOnlyList<T>> GetPagedAsync(int page, int pageSize,
                                                       CancellationToken ct = default)
        => await _set
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<T>> GetPagedAsync(
        Expression<Func<T, bool>>?   predicate,
        Expression<Func<T, object>>? orderBy,
        bool                          ascending = true,
        int                           page      = 1,
        int                           pageSize  = 20,
        CancellationToken             ct        = default)
    {
        IQueryable<T> query = _set;

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    // ──────────────────────────────────────────
    // READ – aggregate
    // ──────────────────────────────────────────

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate,
                                     CancellationToken ct = default)
        => await _set.AnyAsync(predicate, ct);

    public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate,
                                     CancellationToken ct = default)
        => await _set.AllAsync(predicate, ct);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
                                      CancellationToken ct = default)
        => predicate is null
            ? await _set.CountAsync(ct)
            : await _set.CountAsync(predicate, ct);

    // ──────────────────────────────────────────
    // READ – raw IQueryable
    // ──────────────────────────────────────────

    public IQueryable<T> Query()          => _set.AsQueryable();
    public IQueryable<T> QueryNoTracking() => _set.AsNoTracking();

    // ──────────────────────────────────────────
    // WRITE – add
    // ──────────────────────────────────────────

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _set.AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await _set.AddRangeAsync(entities, ct);

    // ──────────────────────────────────────────
    // WRITE – update
    // ──────────────────────────────────────────

    public void Update(T entity)                      => _set.Update(entity);
    public void UpdateRange(IEnumerable<T> entities)  => _set.UpdateRange(entities);

    // ──────────────────────────────────────────
    // WRITE – delete
    // ──────────────────────────────────────────

    public void Delete(T entity)                      => _set.Remove(entity);
    public void DeleteRange(IEnumerable<T> entities)  => _set.RemoveRange(entities);

    public async Task DeleteByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"{typeof(T).Name} with id '{id}' not found.");
        _set.Remove(entity);
    }

    // ──────────────────────────────────────────
    // WRITE – upsert
    // ──────────────────────────────────────────

    public async Task UpsertAsync(T entity, CancellationToken ct = default)
    {
        // Rely on EF's entry state: Detached → Added, Modified → Updated.
        var entry = db.Entry(entity);
        if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Detached)
            await _set.AddAsync(entity, ct);
        else
            _set.Update(entity);
    }

    // ──────────────────────────────────────────
    // SAVE
    // ──────────────────────────────────────────

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await db.SaveChangesAsync(ct);
}

using System.Linq.Expressions;

namespace School.Application.Interfaces;

/// <summary>
/// Generic repository covering all common data-access patterns.
/// T must be a reference type (EF entity class).
/// </summary>
public interface IGenericRepository<T> where T : class
{
    // ──────────────────────────────────────────
    // READ – single
    // ──────────────────────────────────────────

    /// <summary>Returns entity by primary key (Guid), or null.</summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns first entity matching predicate, or null.</summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                  CancellationToken ct = default);

    /// <summary>Returns single entity matching predicate, or null.
    /// Throws if more than one match.</summary>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                   CancellationToken ct = default);

    // ──────────────────────────────────────────
    // READ – collection
    // ──────────────────────────────────────────

    /// <summary>Returns all rows (use carefully on large tables).</summary>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Returns all rows matching predicate.</summary>
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                     CancellationToken ct = default);

    /// <summary>Returns rows matching predicate, ordered, with optional paging.</summary>
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>>?   predicate,
        Expression<Func<T, object>>? orderBy,
        bool                          ascending = true,
        int                           skip      = 0,
        int                           take      = int.MaxValue,
        CancellationToken             ct        = default);

    // ──────────────────────────────────────────
    // READ – paged
    // ──────────────────────────────────────────

    /// <summary>Returns a paged slice of all rows.</summary>
    Task<IReadOnlyList<T>> GetPagedAsync(int page, int pageSize,
                                          CancellationToken ct = default);

    /// <summary>Returns a paged + filtered + ordered slice.</summary>
    Task<IReadOnlyList<T>> GetPagedAsync(
        Expression<Func<T, bool>>?   predicate,
        Expression<Func<T, object>>? orderBy,
        bool                          ascending = true,
        int                           page      = 1,
        int                           pageSize  = 20,
        CancellationToken             ct        = default);

    // ──────────────────────────────────────────
    // READ – aggregate
    // ──────────────────────────────────────────

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int>  CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

    // ──────────────────────────────────────────
    // READ – raw IQueryable (joins, projections, custom includes)
    // ──────────────────────────────────────────

    /// <summary>Tracked queryable – use when you intend to update/delete the results.</summary>
    IQueryable<T> Query();

    /// <summary>No-tracking queryable – use for read-only projections / reports.</summary>
    IQueryable<T> QueryNoTracking();

    // ──────────────────────────────────────────
    // WRITE – add
    // ──────────────────────────────────────────

    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    // ──────────────────────────────────────────
    // WRITE – update  (marks entity Modified in change tracker)
    // ──────────────────────────────────────────

    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    // ──────────────────────────────────────────
    // WRITE – delete
    // ──────────────────────────────────────────

    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task DeleteByIdAsync(Guid id, CancellationToken ct = default);

    // ──────────────────────────────────────────
    // WRITE – bulk convenience (EF change tracker)
    // ──────────────────────────────────────────

    /// <summary>AddOrUpdate: attaches if Id exists, adds otherwise.</summary>
    Task UpsertAsync(T entity, CancellationToken ct = default);

    // ──────────────────────────────────────────
    // SAVE
    // ──────────────────────────────────────────

    /// <summary>Persists all pending changes. Returns affected row count.</summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

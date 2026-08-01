using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if an entity exists by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Guid id);
}

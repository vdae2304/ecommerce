using System.Linq.Expressions;

namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Return a queryable for this entity.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> AsQueryable();

        /// <summary>
        /// Find an entity by its id.
        /// </summary>
        /// <param name="id">Entity ID.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T?> FindByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Return the number of entities that satisfy a condition.
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Test if all entities satisfy a condition.
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Test if any entity satisfies a condition.
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new entity.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new entities.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update the fields of an entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the fields of each entity.
        /// </summary>
        /// <param name="entities">Entities to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete existing entities.
        /// </summary>
        /// <param name="entities">Entities to delete.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}

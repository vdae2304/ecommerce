using Ecommerce.Common.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<T> AsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<T?> FindByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().CountAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AllAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding entity to database");
                throw new Exception("Internal Server Error");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.AddRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding entities to database");
                throw new Exception("Internal Server Error");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating entity from database");
                throw new Exception("Internal Server Error");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.UpdateRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating entities from database");
                throw new Exception("Internal Server Error");
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting entity from database");
                throw new Exception("Internal Server Error");
            }
        }

        /// <inheritdoc/>
        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.RemoveRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting entities from database");
                throw new Exception("Internal Server Error");
            }
        }
    }
}

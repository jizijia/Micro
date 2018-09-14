using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.MySQL
{
    public interface IDbContext
    {

        DbContext DbContext { get; }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Result</returns>
        int SaveChanges();

        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Micro.Core.Data
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 持久化变动内容
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        ///// <summary>
        ///// 开始事务
        ///// </summary>
        ///// <returns></returns>
        //Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// 持久化变动内容
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}

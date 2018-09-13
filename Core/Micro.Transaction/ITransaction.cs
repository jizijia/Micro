using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Transaction
{
    /// <summary>
    /// 事务
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// 事务表示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 版本号
        /// </summary>
        int Version { get; set; }
        /// <summary>
        /// 构建事务流程
        /// </summary>
        /// <returns></returns>
        ITransatcionBuilder Build(string provider, string connectionString);
    }
}

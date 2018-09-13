using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Transaction
{
    /// <summary>
    /// 事务单元
    /// </summary>
    public interface ITransactionUnit<T>
    {
        /// <summary>
        /// 继续执行
        /// </summary>
        /// <returns></returns>
        T Continue();

        /// <summary>
        /// 出现错误的时候回滚
        /// </summary>
        /// <returns></returns>
        bool RollBack(T parameter);
    }
}

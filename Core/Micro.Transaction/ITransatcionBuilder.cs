using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Transaction
{
    public interface ITransatcionBuilder
    {
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="transactionUnit"></param>
        /// <returns></returns>
        ITransatcionBuilder Start(ITransactionUnit transactionUnit);
        /// <summary>
        /// 然后
        /// </summary>
        /// <param name="transactionUnit"></param>
        /// <returns></returns>
        ITransatcionBuilder Then(ITransactionUnit transactionUnit);
        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="transactionUnit"></param>
        /// <returns></returns>
        ITransatcion End(ITransactionUnit transactionUnit);
    }
}

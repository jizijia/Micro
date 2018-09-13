using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Transaction
{
    public class TransactionBuilder : ITransatcionBuilder
    {
        private readonly ITransaction _transaction;
        public TransactionBuilder(ITransaction transaction)
        {
            _transaction = transaction;
        }
        public ITransatcionBuilder End(ITransactionUnit transactionUnit)
        {
            throw new NotImplementedException();
        }

        public ITransatcionBuilder Start(ITransactionUnit transactionUnit)
        {
            throw new NotImplementedException();
        }

        public ITransatcionBuilder Then(ITransactionUnit transactionUnit)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Transaction
{
    public class Transaction : ITransaction
    {
        private readonly List<ITransactionUnit> _transactionUnits; 

        public Transaction()
        {
            _transactionUnits = new List<ITransactionUnit>();
        }
        public Guid Id { get; }

        public int Version { get; set; }

        public ITransatcionBuilder Build(string provider, string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}

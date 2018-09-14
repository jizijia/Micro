using JetBrains.Annotations;
using Micro.Core;
using Micro.Core.Data;
using Micro.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.MySQL
{
    public class UnitOfWork<TEntity> : IUnitOfWork
        where TEntity : class, IEntity
    {
        private readonly ILogger<IRepository<TEntity>> _logger;
        private readonly IDbContext _dbContext;
        public UnitOfWork(IDbContext dbContext, ILogger<IRepository<TEntity>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return await _dbContext.DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                StringBuilder sb = new StringBuilder();
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    sb.AppendLine("Unable to save changes. The department was deleted by another user.");
                }
                else
                {
                    bool isAppendTitle = false;
                    foreach (var item in databaseEntry.Properties)
                    {
                        if (databaseEntry[item.Name].ToString() != entry.CurrentValues[item.Name].ToString())
                        {
                            sb.AppendLine($"\t{item.Name}\tDatabaseValue: {databaseEntry[item.Name]}\t=>\tOriginalValue:{entry.CurrentValues[item.Name]}=>\tClientValue:{entry.CurrentValues[item.Name]}");
                        }
                        if (entry.IsKeySet && item.IsKey())
                        {
                            sb.Insert(0, $"{entry.Entity.GetType().FullName}\t {item.Name}:{databaseEntry[item.Name]}\r\n");
                            isAppendTitle = true;
                        }
                    }
                    if (!isAppendTitle)
                    {
                        sb.Insert(0, $"{entry.Entity.GetType().FullName}\tClientEntiry:{JsonHelper.ToJson(clientValues)}\r\n");
                    }
                }

                sb.Insert(0, $"{ex.Message}\r\n");
                _logger?.LogError($"\r\n{ sb.ToString()}");
                return 0;

            }
            catch (RetryLimitExceededException /* dex */)
            {
                _logger?.LogError("Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return 0;
            }
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _dbContext.DbContext.Database.BeginTransactionAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.DbContext.SaveChanges();
        }
    }
}

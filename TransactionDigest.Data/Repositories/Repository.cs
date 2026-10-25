using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using TransactionDigest.Data.Abstractions;
using TransactionDigest.Data.Context;
using TransactionDigest.Domain;

namespace TransactionDigest.Data.Repositories
{
    /// <summary>
    /// Provide a surface for the business logic to interact with the DbContext so we can mock db calls for the sake of unit testing
    /// Normally I would break this out into multiple repositories for seperation of concerns, but time is limited.
    /// </summary>
    public class Repository(TransactionContext context) : IRepository
    {
        public async Task AddAuditLogs(IEnumerable<TransactionAuditLog> auditLogs)
        {
            await context.TransactionAuditLogs.AddRangeAsync(auditLogs);
        }

        public async Task AddTransactions(IEnumerable<Transaction> transactions)
        {
            await context.Transactions.AddRangeAsync(transactions);
        }
        public async Task<IEnumerable<TransactionAuditLog>> GetAuditLogs(Expression<Func<TransactionAuditLog, bool>>? predicate = null)
        {
            if (predicate == null)
                return await context.TransactionAuditLogs.ToListAsync();
            return await context.TransactionAuditLogs.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(Expression<Func<Transaction, bool>>? predicate = null)
        {
            if (predicate == null)
                return await context.Transactions.ToListAsync();
            return await context.Transactions.Where(predicate).ToListAsync();
        }

        public async Task DeleteAllTransactions()
        {
            var transactions = await context.Transactions.ToListAsync();
            context.Transactions.RemoveRange(transactions);
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}

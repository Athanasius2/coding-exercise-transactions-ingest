using System.Linq.Expressions;
using TransactionDigest.Domain;

namespace TransactionDigest.Data.Abstractions
{
    public interface IRepository
    {
        public Task<IEnumerable<Transaction>> GetTransactions(Expression<Func<Transaction, bool>>? predicate = null);
        public Task AddTransactions(IEnumerable<Transaction> transactions);
        public Task<IEnumerable<TransactionAuditLog>> GetAuditLogs(Expression<Func<TransactionAuditLog, bool>>? predicate = null);
        public Task AddAuditLogs(IEnumerable<TransactionAuditLog> auditLogs);
        public Task DeleteAllTransactions();
        public Task<int> SaveChanges();
    }
}

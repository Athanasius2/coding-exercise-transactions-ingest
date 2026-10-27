using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Application.Dto;
using TransactionDigest.Data.Abstractions;
using TransactionDigest.Domain;

namespace TransactionDigest.Application.Services
{
    public class TransactionService(IRepository repository, IImportService importService, TimeProvider time, ILogger<TransactionService> logger, IMapper mapper) : ITransactionService
    {
        // because TransactionService is scoped, this will be set per http request
        private readonly DateTime _yesterday = time.GetUtcNow().AddDays(-1).DateTime;
        public async Task<IEnumerable<TransactionAuditLogModel>> GetAuditLogs()
        {
            var auditLogs = await repository.GetAuditLogs();
            return mapper.Map<List<TransactionAuditLogModel>>(auditLogs);
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions()
        {
            var transactions = await repository.GetTransactions();
            return mapper.Map<List<TransactionModel>>(transactions);
        }

        public async Task<int> ImportTransactions()
        {
            // 1. import new transactions
            IEnumerable<Transaction> importedTransactions = await importService.ImportTransactions();
            // 2. elminiate imported transactions older than 24 hours
            importedTransactions = importedTransactions.Where(e => e.TransactionTime >= _yesterday);
            // 3. fetch existing, pending transactions
            var existingTransactions = await repository.GetTransactions(e => e.Status == TransactionStatus.Pending);
            // 4. any existing transactions that are older than 24 hours and have not been revoked are finalized
            existingTransactions = FinalizeTransactions(existingTransactions);
            // 5. any existing transactions that do not appear in imported transactions are revoked
            existingTransactions = RevokeTransactions(existingTransactions, importedTransactions);
            // 6. update existing transactions that are pending with new info 
            var newTransactions = await UpdateExisting(existingTransactions, importedTransactions);
            // 7. add imported transactions that do not already exist
            await repository.AddTransactions(newTransactions);
            await repository.SaveChanges();
            return newTransactions.Count();
        }

        public async Task DeleteAllTransactions()
        {
            await repository.DeleteAllTransactions();
            await repository.SaveChanges();
        }

        /// <summary>
        /// Mark pending transactions that are older than 24 hours as finalized
        /// </summary>
        /// <param name="existing"></param>
        /// <returns>List of existing transactions that were not finalized</returns>
        public IEnumerable<Transaction> FinalizeTransactions(IEnumerable<Transaction> existing)
        {
            var finalize = existing.Where(e => e.TransactionTime < _yesterday);
            foreach (var f in finalize)
            {
                f.Status = TransactionStatus.Finalized;
            }
            return existing.Except(finalize);
        }


        /// <summary>
        /// return all transactions from existing that do not exist in imported and are newer than 24 hours
        /// </summary>
        /// <returns>List of existing transactions that have not been finalized or revoked</returns>
        public IEnumerable<Transaction> RevokeTransactions(IEnumerable<Transaction> existing, IEnumerable<Transaction> imported)
        {
            var updatable = GetUpdateable(existing, imported);
            var revoked = existing.Except(updatable);
            foreach (var r in revoked)
            {
                r.Status = TransactionStatus.Revoked;
            }
            return updatable;
        }
        
        /// <summary>
        /// Return a list of existing transactions that also exist in the imported
        /// </summary>
        public IEnumerable<Transaction> GetUpdateable(IEnumerable<Transaction> existing, IEnumerable<Transaction> imported)
        {
            List<Transaction> matching = new List<Transaction>();
            foreach(var t1 in existing)
            {
                var t2 = imported.FirstOrDefault(e => e.TransactionId == t1.TransactionId && e.SourceSystem == t1.SourceSystem);
                if (t2 == null)
                    continue;
                matching.Add(t1);
            }
            return matching;
        }

        /// <summary>
        /// Update transactions that have been ammended
        /// </summary>
        /// <returns>Transactions that do not already exist in the database</returns>
        public async Task<IEnumerable<Transaction>> UpdateExisting(IEnumerable<Transaction> existing, IEnumerable<Transaction> imported)
        {
            List<Transaction> matching = new List<Transaction>();
            foreach(var t1 in existing)
            {
                var t2 = imported.FirstOrDefault(e => e.TransactionId == t1.TransactionId && e.SourceSystem == t1.SourceSystem);
                if (t2 == null)
                    continue;
                var auditLogs = UpdateTransaction(t1, t2);
                await repository.AddAuditLogs(auditLogs);
                matching.Add(t2);
            }
            // return all new transactions 
            return imported.Except(matching);
        }

        /// <summary>
        /// Compare existing transaction with imported and update existing if there are differences
        /// </summary>
        /// <param name="t1">existing</param>
        /// <param name="t2">imported</param>
        /// <returns>return a list of audit logs that document the updates</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<TransactionAuditLog> UpdateTransaction(Transaction t1, Transaction t2)
        {
            if (t2.SourceSystem != t1.SourceSystem || t2.TransactionId != t1.TransactionId)
            {
                throw new ArgumentException(
                    $"Cannot update transaction ({t1.SourceSystem} - {t1.TransactionId}) with data from transaction ({t2.SourceSystem} - {t2.TransactionId})");
            }

            PropertyInfo[] props = t1.GetType().GetProperties();
            var auditLogs = new List<TransactionAuditLog>();

            foreach(var prop in props)
            {
                var currentValue = prop.GetValue(t1);
                var updatedValue = prop.GetValue(t2);

                if (object.Equals(currentValue, updatedValue))
                    continue;

                prop.SetValue(t1, updatedValue);

                auditLogs.Add(new TransactionAuditLog
                {
                    SourceSystem = t1.SourceSystem,
                    TransactionId = t1.TransactionId,
                    FieldName = prop.Name,
                    CurrentValue = currentValue?.ToString(),
                    UpdatedValue = updatedValue?.ToString(),
                    UpdateTime = time.GetUtcNow().DateTime,
                });
            }

            return auditLogs;
        }
    }
}

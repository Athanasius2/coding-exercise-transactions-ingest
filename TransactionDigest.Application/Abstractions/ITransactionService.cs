using TransactionDigest.Application.Dto;

namespace TransactionDigest.Application.Abstractions
{
    public interface ITransactionService
    {
        public Task<IEnumerable<TransactionModel>> GetTransactions();
        public Task<int> ImportTransactions();
        public Task<IEnumerable<TransactionAuditLogModel>> GetAuditLogs();

        /// <summary>
        /// Strictly for testing purposes so I can reset the database without rebuilding it every time
        /// </summary>
        /// <returns></returns>
        public Task DeleteAllTransactions();
    }
}

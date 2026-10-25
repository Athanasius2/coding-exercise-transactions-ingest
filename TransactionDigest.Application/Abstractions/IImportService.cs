using TransactionDigest.Domain;

namespace TransactionDigest.Application.Abstractions
{
    public interface IImportService
    {
        public Task<IEnumerable<Transaction>> ImportTransactions();
    }
}

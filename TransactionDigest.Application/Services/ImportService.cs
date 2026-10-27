using TransactionDigest.Application.Abstractions;
using TransactionDigest.Domain;

namespace TransactionDigest.Application.Services
{
    public class ImportService : IImportService
    {
        public Task<IEnumerable<Transaction>> ImportTransactions()
        {
            throw new NotImplementedException();
        }
    }
}

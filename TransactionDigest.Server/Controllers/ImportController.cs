using Microsoft.AspNetCore.Mvc;
using TransactionDigest.Application;
using TransactionDigest.Application.Abstractions;

namespace TransactionDigest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController(ITransactionService transactions, ILogger<ImportController> logger, TimeProvider time) : TransactionControllerBase<ImportController>(logger)
    {
        public async Task<OperationResult<int>> Post()
        {
            try
            {
                _logger.LogInformation($"POST call to api/import at {time.GetUtcNow().DateTime}");
                int added = await transactions.ImportTransactions();
                return new() { Result = added };
            }
            catch(Exception ex)
            {
                return HandleError<int>(ex, "Failed to import new transactions");
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionDigest.Application;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Application.Dto;

namespace TransactionDigest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionService transactions, ILogger<TransactionsController> logger, TimeProvider time) : TransactionControllerBase<TransactionsController>(logger)
    {
        public async Task<OperationResult<IEnumerable<TransactionModel>>> Get()
        {
            try
            {
                _logger.LogInformation($"GET call to api/transactions at {time.GetUtcNow().DateTime}");
                var tr = await transactions.GetTransactions();
                return new() { Result = tr };
            }
            catch (Exception ex)
            {
                return HandleError<IEnumerable<TransactionModel>>(ex, "Failed to fetch transactions");
            }
        }

        [Route("auditlogs")]
        [HttpGet]
        public async Task<OperationResult<IEnumerable<TransactionAuditLogModel>>> AuditLogs()
        {
            try
            {
                _logger.LogInformation($"GET call to api/transactions/auditlogs at {time.GetUtcNow().DateTime}");
                var logs = await transactions.GetAuditLogs();
                return new() { Result = logs };
            }
            catch(Exception ex)
            {
                return HandleError<IEnumerable<TransactionAuditLogModel>>(ex, "Failed to fetch transaction audit logs");
            }
        }

        /// <summary>
        /// Purely for testing purposes so I can reset the database
        /// </summary>
        [HttpDelete]
        public async Task<OperationResult> Delete()
        {
            try
            {
                _logger.LogInformation($"GET call to api/transactions/deleteall at {time.GetUtcNow().DateTime}");
                await transactions.DeleteAllTransactions();
                return new OperationResult();
            }
            catch(Exception ex)
            {
                return HandleError(ex, "Failed to delete transactions");
            }
        }
    }
}

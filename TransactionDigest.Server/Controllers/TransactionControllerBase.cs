using Microsoft.AspNetCore.Mvc;
using TransactionDigest.Application;

namespace TransactionDigest.Server.Controllers
{
    public class TransactionControllerBase<T>(ILogger<T> logger) : ControllerBase
    {
        protected readonly ILogger<T> _logger = logger;

        protected OperationResult<TResult> HandleError<TResult>(Exception ex, string message)
        {
            logger.LogError(ex, message);
            return new()
            {
                Success = false,
                ErrorMessage = message,
            };
        }

        protected OperationResult HandleError(Exception ex, string message)
        {
            logger.LogError(ex, message);
            return new()
            {
                Success = false,
                ErrorMessage = message,
            };
        }
    }
}

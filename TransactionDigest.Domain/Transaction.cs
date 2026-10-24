using System.Reflection;

namespace TransactionDigest.Domain
{
    public class Transaction
    {
        public required string SourceSystem { get; set; }
        public required int TransactionId { get; set; }
        public string? CardNumber { get; set; }
        public required string LocationCode { get; set; }
        public required string ProductName { get; set; }
        public required decimal Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public TransactionStatus Status { get; set; }

        public IEnumerable<TransactionAuditLog> Update(Transaction transaction)
        {
            if (transaction.SourceSystem != SourceSystem || transaction.TransactionId != TransactionId)
            {
                throw new InvalidOperationException(
                    $"Cannot update transaction ({SourceSystem} - {TransactionId}) with data from transaction ({transaction.SourceSystem} - {transaction.TransactionId})");
            }

            PropertyInfo[] props = GetType().GetProperties();
            var auditLogs = new List<TransactionAuditLog>();

            foreach(var prop in props)
            {
                var currentValue = prop.GetValue(this);
                var updatedValue = prop.GetValue(transaction);
                if (currentValue == updatedValue)
                    continue;

                prop.SetValue(this, updatedValue);

                auditLogs.Add(new TransactionAuditLog
                {
                    SourceSystem = SourceSystem,
                    TransactionId = TransactionId,
                    FieldName = prop.Name,
                    CurrentValue = currentValue?.ToString(),
                    UpdatedValue = updatedValue?.ToString(),
                    UpdateTime = DateTime.UtcNow
                });
            }

            return auditLogs;
        }
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Revoked = 1,
        Finalized = 2,
    }
}

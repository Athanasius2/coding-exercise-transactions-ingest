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
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Revoked = 1,
        Finalized = 2,
    }
}

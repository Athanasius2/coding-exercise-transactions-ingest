namespace TransactionDigest.Domain
{
    public class TransactionAuditLog
    {
        public int Id { get; set; }
        public required string SourceSystem { get; set; }
        public required int TransactionId { get; set; }
        public Transaction? Transaction { get; set; }
        public required string FieldName { get; set; }
        public required string? CurrentValue { get; set; }
        public required string? UpdatedValue { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

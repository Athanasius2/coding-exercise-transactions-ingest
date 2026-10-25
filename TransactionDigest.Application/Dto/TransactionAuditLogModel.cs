namespace TransactionDigest.Application.Dto
{
    public class TransactionAuditLogModel
    {
        public int Id { get; set; }
        public required string SourceSystem { get; set; }
        public required int TransactionId { get; set; }
        public TransactionModel? Transaction { get; set; }
        public required string FieldName { get; set; }
        public required string? CurrentValue { get; set; }
        public required string? UpdatedValue { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

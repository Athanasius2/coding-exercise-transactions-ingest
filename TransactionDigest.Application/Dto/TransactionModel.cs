using TransactionDigest.Domain;

namespace TransactionDigest.Application.Dto
{
    public class TransactionModel
    {
        public required string SourceSystem { get; set; }
        public required int TransactionId { get; set; }
        public string? CardNumber { get; set; }
        public required string LocationCode { get; set; }
        public required string ProductName { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime TransactionTime { get; set; }
        public TransactionStatus Status { get; set; }
    }
}

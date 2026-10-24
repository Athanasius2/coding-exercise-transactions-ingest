using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionDigest.Domain
{
    public class Transaction
    {
        public required string SourceSystem { get; set; }
        public required int TransactionId { get; set; }
        public string? CardNumber { get; set; }
        public required string ProductName { get; set; }

    }
}

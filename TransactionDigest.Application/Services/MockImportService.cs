using Microsoft.Extensions.Time.Testing;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Domain;

namespace TransactionDigest.Application.Services
{
    public class MockImportService(FakeTimeProvider time) : IImportService
    {
        private static readonly List<List<Transaction>> s_dataSet = [
            [
                new()
                {
                    SourceSystem = "Store Uptown",
                    TransactionId = 100001,
                    CardNumber = "***1111",
                    LocationCode = "UPTOWN",
                    ProductName = "Wireless Mouse",
                    Amount = 24.99M,
                    TransactionTime = DateTime.Parse("2026-02-27T08:15:30Z")
                },
                new()
                {
                    SourceSystem = "Store Downtown",
                    TransactionId = 100001,
                    CardNumber = "***0002",
                    LocationCode = "DOWNTOWN",
                    ProductName = "Mechanical Keyboard",
                    Amount = 89.99M,
                    TransactionTime = DateTime.Parse("2026-02-27T09:45:12Z")
                },
                new()
                {
                    SourceSystem = "Online",
                    TransactionId = 100001,
                    CardNumber = "***4444",
                    LocationCode = "ONLINE",
                    ProductName = "USB-C Docking Station",
                    Amount = 129.95M,
                    TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
                }
            ],
            [
                // Transaction stays the same
                new()
                {
                    SourceSystem = "Store Uptown",
                    TransactionId = 100001,
                    CardNumber = "***1111",
                    LocationCode = "UPTOWN",
                    ProductName = "Wireless Mouse",
                    Amount = 24.99M,
                    TransactionTime = DateTime.Parse("2026-02-27T08:15:30Z")
                },
                // Transaction is modified
                new()
                {
                    SourceSystem = "Store Downtown",
                    TransactionId = 100001,
                    CardNumber = "***0002",
                    LocationCode = "DOWNTOWN",
                    ProductName = "Laptop",
                    Amount = 200.99M,
                    TransactionTime = DateTime.Parse("2026-02-27T09:45:12Z")
                },
                // new transaction is inserted, other transaction is revoked
                new()
                {
                    SourceSystem = "Online",
                    TransactionId = 100002,
                    CardNumber = "***4444",
                    LocationCode = "ONLINE",
                    ProductName = "USB-C Docking Station",
                    Amount = 129.95M,
                    TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
                }
            ]
        ];

        private static int s_batchNumber = 0;

        private static List<DateTime> s_mockTimes = [
            DateTime.Parse("2026-02-27T12:00:05Z"),
            DateTime.Parse("2026-02-27T13:00:05Z"),
            DateTime.Parse("2026-02-28T13:00:05Z"),
            ];

        public async Task<IEnumerable<Transaction>> ImportTransactions()
        {
            if (s_batchNumber >= s_dataSet.Count)
            {
                s_batchNumber = 0;
            }

            time.AdjustTime(s_mockTimes[s_batchNumber]);
            return s_dataSet[s_batchNumber++];
        }
    }
}

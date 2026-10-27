using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using TransactionDigest.Application;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Application.Services;
using TransactionDigest.Data.Abstractions;
using TransactionDigest.Domain;

namespace TransactionDigest.Test
{
    [TestClass]
    public class TransactionService_UpdateTransaction
    {
        private readonly TransactionService service;
        private readonly IRepository repository;
        private readonly IImportService importService;
        private readonly FakeTimeProvider fakeTimeProvider;
        private readonly ILogger<TransactionService> logger;
        private readonly IMapper mapper;
        public TransactionService_UpdateTransaction()
        {
            repository = new Mock<IRepository>().Object;
            importService = new Mock<IImportService>().Object;
            fakeTimeProvider = new FakeTimeProvider();
            logger = new Mock<ILogger<TransactionService>>().Object;
            var loggerFactory = new Mock<ILoggerFactory>().Object;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>(), loggerFactory);
            mapper = new Mapper(config);
            service = new TransactionService(repository, importService, fakeTimeProvider, logger, mapper);
        }

        [TestMethod]
        public void UpdateTransaction_DifferentTransactionKeys_ThrowsArgumentException()
        {
            Transaction t1 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };
            Transaction t2 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100002,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var exception = Assert.ThrowsExactly<ArgumentException>(() => 
                service.UpdateTransaction(t1, t2));
        }

        [TestMethod]
        public void UpdateTransaction_SameTransaction_NoAuditLogs()
        {
            Transaction t1 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };
            Transaction t2 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var logs = service.UpdateTransaction(t1, t2);

            Assert.IsEmpty(logs);
        }
        public void UpdateTransaction_SameTransaction_Same()
        {
            Transaction t1 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };
            Transaction t2 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var logs = service.UpdateTransaction(t1, t2);

            Assert.IsEmpty(logs);
        }

        [TestMethod]
        public void UpdateTransaction_DifferentTransaction_TwoAuditLogs()
        {
            Transaction t1 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };
            Transaction t2 = new()
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "Laptop",
                Amount = 1000M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var logs = service.UpdateTransaction(t1, t2);

            Assert.AreEqual(2, logs.Count());
        }
    }
}

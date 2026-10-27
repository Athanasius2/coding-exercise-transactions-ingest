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
    public class TransactionService_ImportTransactions
    {
        private TransactionService service;
        private readonly Mock<IRepository> repository = new();
        private readonly Mock<IImportService> importService = new();
        private readonly FakeTimeProvider time = new();
        private readonly Mock<ILogger<TransactionService>> logger = new();
        private readonly IMapper mapper;
        public TransactionService_ImportTransactions()
        {
            var loggerFactory = new Mock<ILoggerFactory>().Object;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>(), loggerFactory);
            mapper = new Mapper(config);
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);
        }

        [TestMethod]
        public async Task ImportTransactions_24HoursOld_TransactionFinalized()
        {
            time.AdjustTime(DateTime.Parse("2026-02-27T12:00:00Z"));
            var t1 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var t2 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z"),
                Status = TransactionStatus.Pending
            };

            importService.SetupSequence(e => e.ImportTransactions())
                .ReturnsAsync([t1])
                .ReturnsAsync([]);

            repository.SetupSequence(e => e.GetTransactions(e => e.Status == TransactionStatus.Pending))
                .ReturnsAsync([])
                .ReturnsAsync([t2]);

            // Create service for first request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);

            int result = await service.ImportTransactions();
            Assert.AreEqual(1, result);
            time.AdjustTime(DateTime.Parse("2026-02-28T12:00:00Z"));

            // Service is scoped so we need to create a new instance to simulate a separate request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);
            result = await service.ImportTransactions();
            Assert.AreEqual(0, result);
            Assert.AreEqual(TransactionStatus.Finalized, t2.Status);
        }

        [TestMethod]
        public async Task ImportTransactions_TransactionRemoved_TransactionRevoked()
        {
            time.AdjustTime(DateTime.Parse("2026-02-27T12:00:00Z"));
            var t1 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var t2 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z"),
                Status = TransactionStatus.Pending
            };

            importService.SetupSequence(e => e.ImportTransactions())
                .ReturnsAsync([t1])
                .ReturnsAsync([]);

            repository.SetupSequence(e => e.GetTransactions(e => e.Status == TransactionStatus.Pending))
                .ReturnsAsync([])
                .ReturnsAsync([t2]);

            // Create service for first request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);

            int result = await service.ImportTransactions();
            Assert.AreEqual(1, result);
            time.AdjustTime(DateTime.Parse("2026-02-27T13:00:00Z"));

            // Service is scoped so we need to create a new instance to simulate a separate request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);
            result = await service.ImportTransactions();
            Assert.AreEqual(0, result);
            Assert.AreEqual(TransactionStatus.Revoked, t2.Status);

            
        }

        [TestMethod]
        public async Task ImportTransactions_TransactionChanged_TransactionUpdated()
        {
            time.AdjustTime(DateTime.Parse("2026-02-27T12:00:00Z"));
            var t1 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var t1_1 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "LAPTOP",
                Amount = 1000M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z")
            };

            var t2 = new Transaction
            {
                SourceSystem = "Online",
                TransactionId = 100001,
                CardNumber = "***4444",
                LocationCode = "ONLINE",
                ProductName = "USB-C Docking Station",
                Amount = 129.95M,
                TransactionTime = DateTime.Parse("2026-02-27T11:20:05Z"),
                Status = TransactionStatus.Pending
            };

            importService.SetupSequence(e => e.ImportTransactions())
                .ReturnsAsync([t1])
                .ReturnsAsync([t1_1]);

            repository.SetupSequence(e => e.GetTransactions(e => e.Status == TransactionStatus.Pending))
                .ReturnsAsync([])
                .ReturnsAsync([t2]);

            // Create service for first request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);

            int result = await service.ImportTransactions();
            Assert.AreEqual(1, result);
            time.AdjustTime(DateTime.Parse("2026-02-27T13:00:00Z"));

            // Service is scoped so we need to create a new instance to simulate a separate request
            service = new TransactionService(repository.Object, importService.Object, time, logger.Object, mapper);
            result = await service.ImportTransactions();
            Assert.AreEqual(0, result);
            Assert.AreEqual("LAPTOP", t2.ProductName);
            Assert.AreEqual(1000M, t2.Amount);

            
        }
    }
}

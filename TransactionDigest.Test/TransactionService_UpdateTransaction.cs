using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Application.Services;
using TransactionDigest.Data.Abstractions;

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
            service = new TransactionService(repository, importService, fakeTimeProvider, logger, mapper);
        }
    }
}

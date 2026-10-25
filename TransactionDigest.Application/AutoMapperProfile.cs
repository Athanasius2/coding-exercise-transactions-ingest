using AutoMapper;
using TransactionDigest.Application.Dto;
using TransactionDigest.Domain;

namespace TransactionDigest.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Transaction, TransactionModel>();
            CreateMap<TransactionAuditLog, TransactionAuditLogModel>();
        }
    }
}

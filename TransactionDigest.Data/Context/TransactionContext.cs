using Microsoft.EntityFrameworkCore;
using TransactionDigest.Data.ModelConfig;
using TransactionDigest.Domain;

namespace TransactionDigest.Data.Context
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionAuditLog> TransactionAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionMap());
            modelBuilder.ApplyConfiguration(new TransactionAuditLogMap());
        }
    }
}

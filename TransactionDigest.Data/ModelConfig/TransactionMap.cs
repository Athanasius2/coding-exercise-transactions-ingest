using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionDigest.Domain;

namespace TransactionDigest.Data.ModelConfig
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey("SourceSystem", "TransactionId");

            builder.Property(e => e.SourceSystem).HasMaxLength(20);
            builder.Property(e => e.CardNumber).HasMaxLength(4);
            builder.Property(e => e.LocationCode).HasMaxLength(20);
            builder.Property(e => e.ProductName).HasMaxLength(20);
            builder.Property(e => e.Amount).HasPrecision(18, 2);
            builder.Property(e => e.Status).HasDefaultValue(TransactionStatus.Pending);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionDigest.Domain;

namespace TransactionDigest.Data.ModelConfig
{
    public class TransactionAuditLogMap : IEntityTypeConfiguration<TransactionAuditLog>
    {
        public void Configure(EntityTypeBuilder<TransactionAuditLog> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Transaction).WithMany().HasForeignKey("SourceSystem", "TransactionId");
            builder.Property(e => e.FieldName).HasMaxLength(50);
            builder.Property(e => e.CurrentValue).HasMaxLength(20);
            builder.Property(e => e.UpdatedValue).HasMaxLength(20);
            builder.Property(e => e.UpdateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}

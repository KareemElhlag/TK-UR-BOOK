using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Infrastructure.Persistence.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsOne(p => p.PaidAmount, mony =>
            {
                mony.Property(m => m.Amount).
                HasColumnName("PaidAmount").HasPrecision(18, 2);
                mony.Property(m => m.Currency)
                .HasColumnName("Currency").HasMaxLength(3)
                .HasConversion<String>();
            });
        }
    }
}

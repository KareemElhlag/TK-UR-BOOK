using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Infrastructure.Persistence.Converters;

namespace TK_UR_BOOK.Infrastructure.Persistence.Configurations
{
    public class BookConfigrations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasConversion(new StronglyTypedIdConverter<BookId, Guid>());
            builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Author).IsRequired().HasMaxLength(100);
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            builder.OwnsOne(builder => builder.Price, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount).HasColumnName("Price").IsRequired().HasPrecision(18, 2);
                moneyBuilder.Property(m => m.Currency)
                .HasColumnName("Currency").IsRequired().HasMaxLength(3)
                .HasConversion<String>();

            });
            builder.HasQueryFilter(b => !b.IsDeleted);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Infrastructure.Persistence.Configurations
{
    public class PermissionConfigration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                  .HasConversion(new Converters.StronglyTypedIdConverter<PermissionId, int>());
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(p => p.Code).IsUnique();
            //// Seed initial permissions
            builder.HasData(
                new Permission(new PermissionId(1), "User Management", "USER_MANAGEMENT"),
                new Permission(new PermissionId(2), "Book Management", "BOOK_MANAGEMENT"),
                new Permission(new PermissionId(3), "Full Access", "Admin_Access")

                );
        }
    }
}

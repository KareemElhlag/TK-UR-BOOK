using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Infrastructure.Persistence.Converters;

namespace TK_UR_BOOK.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasConversion(new StronglyTypedIdConverter<UserId, Guid>());
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.HasMany(u => u.Groups).WithMany(g => g.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroup",
                    j => j.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "GroupId");
                        j.ToTable("UserGroups");
                    }
                );
            builder.HasMany(u => u.RefreshTokens).WithOne(rt => rt.User).HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasData(

                new User(
        new UserId(new Guid("4ca9451d-945d-4965-8a41-6716e90f790c")), // الـ ID الثابت هنا
        "admin",
        "Admin@123.TKURBOOK.com",
        "admin@123"
    )
                {
                    CreatedAt = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc),
                }
                );
        }
    }
}


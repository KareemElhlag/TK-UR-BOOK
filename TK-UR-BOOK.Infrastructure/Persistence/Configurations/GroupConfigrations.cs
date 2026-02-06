using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Infrastructure.Persistence.Converters;

namespace TK_UR_BOOK.Infrastructure.Persistence.Configurations
{
    public class GroupConfigrations : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Id).HasConversion(new StronglyTypedIdConverter<GroupId>());
            builder.HasMany(g => g.Permissions).WithMany().UsingEntity(j =>
            {
                j.ToTable("GroupPermissions");

                j.Property("PermissionsId")
                 .HasConversion(new StronglyTypedIdConverter<PermissionId>());

                j.Property("GroupsId")
                 .HasConversion(new StronglyTypedIdConverter<GroupId>());
            });

        }
    }
}

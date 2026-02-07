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
            builder.Property(g => g.Id).HasConversion(new StronglyTypedIdConverter<GroupId , int>());
            builder.HasMany(g => g.Permissions).WithMany().UsingEntity(j =>
            {
                j.ToTable("GroupPermissions");

                j.Property<PermissionId>("PermissionsId")
                 .HasConversion(new StronglyTypedIdConverter<PermissionId, int>());

                j.Property<GroupId>("GroupsId")
                 .HasConversion(new StronglyTypedIdConverter<GroupId, int>());
            });

            builder.HasData(
                 new Group(new GroupId(1) , "Super Admin", "this group have a super control Full Access for system"),
                 new Group(new GroupId(3) , "Pubisher", "Pepole or office to sherd a books for sale"),
                 new Group(new GroupId(2) , "User", "For a normal user")

                );


        }
    }
}

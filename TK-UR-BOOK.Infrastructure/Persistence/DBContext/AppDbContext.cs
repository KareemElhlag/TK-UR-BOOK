using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Domain.Sp_Interface;
using TK_UR_BOOK.Infrastructure.Persistence.Converters;

namespace TK_UR_BOOK.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        DbSet<Book> Books => Set<Book>();
        DbSet<User> Users => Set<User>();
        DbSet<Group> Groups => Set<Group>();
        DbSet<Category> Categories => Set<Category>();
        DbSet<Favorite> Favorites => Set<Favorite>();

        DbSet<Purchase> Purchases => Set<Purchase>();
        DbSet<Rating> Ratings => Set<Rating>();
        DbSet<UserActivity> UserActivities => Set<UserActivity>();
        DbSet<Permission> Permissions => Set<Permission>();
        DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            OnBeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// for checking the state of entities before saving changes to the database.
        /// It looks for entities that implement the IAuditable interface and are either being added or modified. For those entities,
        /// it sets the CreatedAt property when they are added and updates the LastModifiedAt property for both added and modified entities. 
        /// ///This ensures that audit information is automatically maintained for relevant entities in the application.
        /// </summary>
        public void OnBeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is IAuditable &&
            (e.State == EntityState.Added || e.State == EntityState.Modified));
            var currentTime = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        if (auditableEntity.CreatedAt == default)
                        {
                            auditableEntity.CreatedAt = currentTime;
                        }
                        auditableEntity.LastModifiedAt = currentTime;

                    }
                    else if (entry.State == EntityState.Modified)
                    {

                        auditableEntity.LastModifiedAt = currentTime;
                    }
                    entry.Property("CreatedAt").IsModified = entry.State == EntityState.Added;
                }
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder modelConfiguration)
        {
            modelConfiguration.Properties<BookId>()
            .HaveConversion<StronglyTypedIdConverter<BookId, Guid>>();
            modelConfiguration.Properties<UserId>()
        .HaveConversion<StronglyTypedIdConverter<UserId, Guid>>();



        }
    }

}
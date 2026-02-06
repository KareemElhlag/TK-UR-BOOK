
using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Domain.Entities;

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
        DbSet<Rating> ratings => Set<Rating>();
        DbSet<UserActivity> UserActivities => Set<UserActivity>();
        DbSet<Permission> Permissions => Set<Permission>();
        DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}

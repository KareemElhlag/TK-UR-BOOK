using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TK_UR_BOOK.Infrastructure.Persistence.DBContext;

namespace TK_UR_BOOK.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServiceDescriptors(this IServiceCollection services , IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString , b => b.MigrationsAssembly(typeof(AppContext).Assembly.f));
            return services;
        }

       
    }
}

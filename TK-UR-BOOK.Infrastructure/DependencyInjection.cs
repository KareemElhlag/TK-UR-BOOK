using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.Services;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Application.Validations.QueryValidator;
using TK_UR_BOOK.Infrastructure.Persistence;
using TK_UR_BOOK.Infrastructure.Persistence.DBContext;
using TK_UR_BOOK.Infrastructure.Repositories;

namespace TK_UR_BOOK.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServiceDescriptors(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString,
                x => x.MigrationsAssembly("TK-UR-BOOK.Infrastructure")));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<GetBookQueryValidator>();
            services.AddScoped<GetBooksQueryHandler>();
            return services;
        }


    }
}

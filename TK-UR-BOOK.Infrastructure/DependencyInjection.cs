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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            services.AddScoped<IHashingPassword , PasswordHasher>();
            var Jwt = configuration.GetSection("Jwt");
            var secretKey = Jwt["Key"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Jwt["Issuer"],
                    ValidAudience = Jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey!)),
                    ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }


    }
}

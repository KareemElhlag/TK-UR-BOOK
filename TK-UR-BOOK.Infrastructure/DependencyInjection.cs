using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.Services;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Application.UseCases.Payment;
using TK_UR_BOOK.Application.Validations.QueryValidator;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Sp_Interface;
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
            services.AddScoped<IHashingPassword , PasswordHasher>();
            var Jwt = configuration.GetSection("Jwt");
            var secretKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT Secret Key is missing! Make sure it's in user-secrets or appsettings.json");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Jwt["Issuer"],
                    ValidAudience = Jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<PaymentWebhookCommandHandler>();
            services.AddScoped<PaymentWebhookCommandHandler>();


            return services;
        }


    }
}

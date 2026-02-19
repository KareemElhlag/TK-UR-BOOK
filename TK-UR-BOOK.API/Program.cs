using System.Text.Json.Serialization;
using Serilog;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Infrastructure;
using TK_UR_BOOK.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.ServiceDescriptors(builder.Configuration);
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TK_UR_BOOK API V1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting the Web Host...");
    app.Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "The Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
app.Run();

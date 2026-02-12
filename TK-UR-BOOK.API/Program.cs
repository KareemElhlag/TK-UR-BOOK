using Serilog;
using TK_UR_BOOK.Application.Services;
using TK_UR_BOOK.Controllers;
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


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TK_UR_BOOK API V1");
        c.RoutePrefix = string.Empty; // عشان يفتح Swagger أول ما تشغل المشروع [cite: 2026-02-09]
    });
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization(); // ضيف دي لو ناوي تستخدم صلاحيات لاحقاً
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
    Log.CloseAndFlush(); // تأكيد حفظ الـ Logs [cite: 2026-02-10]
}
app.Run();

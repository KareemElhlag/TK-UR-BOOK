using TK_UR_BOOK.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.ServiceDescriptors(builder.Configuration);

app.MapGet("/", () => "Hello World!");

app.Run();

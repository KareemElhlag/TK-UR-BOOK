using TK_UR_BOOK.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ServiceDescriptors(builder.Configuration);


var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();

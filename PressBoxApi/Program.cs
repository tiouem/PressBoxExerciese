using System.Reflection;
using Microsoft.Extensions.Options;
using PressBoxApi.Common;
using PressBoxApi.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DataSourceSettings>(builder.Configuration.GetSection("DataSourceSettings"));
builder.Services.AddHttpClient<IDataService, DataService>((serviceProvider, client) =>
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<DataSourceSettings>>().Value;

    client.BaseAddress = new Uri("https://psbase.azureedge.net/interview/");
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.CustomSchemaIds(s => s.FullName?.Replace("+", ".")); });

builder.Services.AddCors(p =>
    p.AddPolicy("all", builder => { builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); }));
var app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

namespace PressBoxApi
{
    public partial class Program
    {
    }
}
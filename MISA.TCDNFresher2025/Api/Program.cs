using Core.Interfaces.Repositories;
using MISA.Core.Interfaces.Services;
using MISA.Core.Middlewares;
using MISA.Core.Services;
using MISA.Infrastructure.Repositories;
using MISA.Infrastructure.Utils;

var builder = WebApplication.CreateBuilder(args);

// Config Dapper to match underscores
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



// Config DI

builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IExcelExporterService, ClosedXMLExcelExporter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMISAExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();

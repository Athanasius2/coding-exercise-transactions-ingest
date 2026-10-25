using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using TransactionDigest.Application;
using TransactionDigest.Application.Abstractions;
using TransactionDigest.Application.Services;
using TransactionDigest.Data.Abstractions;
using TransactionDigest.Data.Context;
using TransactionDigest.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string connectionString = builder.Configuration.GetConnectionString("Default") 
    ?? throw new Exception("Connection string not configured in appconfig.json");

builder.Services.AddDbContext<TransactionContext>(options =>
{
    options.UseSqlite(connectionString);
});
if (builder.Environment.IsDevelopment())
{
    var fakeTime = new FakeTimeProvider();
    fakeTime.AdjustTime(DateTime.Parse("2026-02-27T12:00:05Z"));
    builder.Services.AddSingleton((TimeProvider)fakeTime);
    builder.Services.AddSingleton(fakeTime);
}
else
{
    builder.Services.AddSingleton(TimeProvider.System);
}
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IImportService, MockImportService>();
builder.Services.AddAutoMapper(options => options.AddProfile(new AutoMapperProfile()));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

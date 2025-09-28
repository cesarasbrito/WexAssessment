
using Microsoft.EntityFrameworkCore;
using Wex.Infrastructure.Data;
using Wex.Infrastructure.Repositories;
using Wex.Domain.Repositories;
using Wex.Application.Services;
using Wex.Infrastructure.External;



var builder = WebApplication.CreateBuilder(args);

string dbPath;
if (builder.Environment.IsDevelopment())
{
    dbPath = "wex.db"; // local
}
else
{
    Directory.CreateDirectory("/app/data"); // garante que exista
    dbPath = "/app/data/wex.db";
}

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}"));

// dependency injection
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<TransactionService>();

builder.Services.AddHttpClient<ITreasuryApiClient, TreasuryApiClient>();

builder.Services.AddControllers();

var app = builder.Build();

//apply migrartions
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();
app.Run();
using Microsoft.EntityFrameworkCore;
using Wex.Domain.Entities;

namespace Wex.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; } = null!;
}

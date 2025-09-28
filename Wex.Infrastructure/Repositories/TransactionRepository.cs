using Microsoft.EntityFrameworkCore;
using Wex.Domain.Entities;
using Wex.Domain.Repositories;
using Wex.Infrastructure.Data;

namespace Wex.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
    }
}

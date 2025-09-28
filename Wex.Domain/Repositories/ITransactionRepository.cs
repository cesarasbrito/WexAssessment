using Wex.Domain.Entities;

namespace Wex.Domain.Repositories;

public interface ITransactionRepository
{
    Task SaveAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
}
using Wex.Domain.Entities;
using Wex.Domain.Repositories;
using Wex.Application.Dtos;
using Wex.Infrastructure.External;

namespace Wex.Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly ITreasuryApiClient _treasuryApi;

    public TransactionService(ITransactionRepository repository, ITreasuryApiClient treasuryApi)
    {
        _repository = repository;
        _treasuryApi = treasuryApi;
    }

    public async Task<Guid> CreateTransaction(string description, DateTime date, decimal amountUsd)
    {
        var transaction = new Transaction(description, date, amountUsd);
        await _repository.SaveAsync(transaction);
        return transaction.Id;
    }

    public async Task<TransactionDto?> GetTransaction(Guid id, string? country = null)
    {
        var t = await _repository.GetByIdAsync(id);
        if (t == null) return null;

        decimal? rate = null;
        decimal? converted = null;

        if (!string.IsNullOrWhiteSpace(country))
        {
            rate = await _treasuryApi.GetExchangeRateAsync(t.TransactionDate, country);
            Console.WriteLine($"Api fiscaldstaa return {rate}");
            converted = rate.HasValue ? Math.Round(t.AmountUsd * rate.Value, 2) : null;
        }

        return new TransactionDto(
            t.Id,
            t.Description,
            t.TransactionDate,
            t.AmountUsd,
            rate,
            converted
        );
    }
}

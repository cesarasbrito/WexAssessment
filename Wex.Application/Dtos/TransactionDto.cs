namespace Wex.Application.Dtos;

public record TransactionDto(
    Guid Id,
    string Description,
    DateTime TransactionDate,
    decimal AmountUsd,
    decimal? ExchangeRate = null,
    decimal? ConvertedAmount = null
);
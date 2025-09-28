namespace Wex.Infrastructure.External;

public interface ITreasuryApiClient
{
    Task<decimal?> GetExchangeRateAsync(DateTime purchaseDate, string country);
}
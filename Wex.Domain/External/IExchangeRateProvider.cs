namespace Wex.Domain.External
{
    public interface IExchangeRateProvider
    {
        Task<decimal?> GetExchangeRateAsync(DateTime date, string targetCurrency = "BRL");
    }
}
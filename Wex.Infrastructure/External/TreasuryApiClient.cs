
using System.Net.Http;
using System.Net.Http.Json;

using System.Globalization;
using System.Text.Json.Serialization;

namespace Wex.Infrastructure.External;

public class TreasuryApiClient : ITreasuryApiClient
{
    private readonly HttpClient _http;

    public TreasuryApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<decimal?> GetExchangeRateAsync(DateTime purchaseDate, string country)
    {
        // api format date : yyyy-MM-dd
        string formattedPurchaseDate = purchaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        DateTime minDate = purchaseDate.AddMonths(-6);
        string formattedMinDate = minDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        string url =
            $"https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange" +
            $"?fields=country_currency_desc,exchange_rate,record_date" +
            $"&filter=country_currency_desc:in:({country}),record_date:lte:{formattedPurchaseDate},record_date:gte:{formattedMinDate}" +
            $"&sort=-record_date" + // order first by date desc
            $"&format=json";


        Console.WriteLine($"DEBUG: Fetching URL: {url}");

        var response = await _http.GetFromJsonAsync<TreasuryResponse>(url);

        Console.WriteLine($"DEBUG: response = {response}");

        if (response?.Data == null || response.Data.Length == 0)
            return null;

        var record = response.Data.FirstOrDefault();

        if (record == null) return null;

        if (decimal.TryParse(record.OfficialRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
        {
            Console.WriteLine($"DEBUG: Usando taxa {rate} de {record.RecordDate}");
            return rate;
        }
        return null;
    }

    public class TreasuryResponse
    {
        [JsonPropertyName("data")]
        public TreasuryRecord[] Data { get; set; }
    }
    public record TreasuryRecord(
        [property: JsonPropertyName("country_currency_desc")] string CountryCurrencyDesc,
        [property: JsonPropertyName("exchange_rate")] string OfficialRate,
        [property: JsonPropertyName("record_date")] string RecordDate
    );
}
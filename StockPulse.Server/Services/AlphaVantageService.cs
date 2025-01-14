using StockPulse.Server.Models;
using System.Text.Json;

namespace StockPulse.Server.Services;

public class AlphaVantageService(HttpClient httpClient, IConfiguration configuration)
{
    private readonly string _apiKey = configuration["AlphaVantage:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "AlphaVantage API key is missing.");

    public async Task<Stock?> GetStockPriceAsync(string symbol)
    {
        var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=1min&apikey={_apiKey}";
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(content);
        var timeSeries = jsonDocument.RootElement.GetProperty("Time Series (1min)");
        var latestEntry = timeSeries.EnumerateObject().First();
        var timestamp = latestEntry.Name;
        var data = latestEntry.Value;
        var closePrice = decimal.Parse(data.GetProperty("4. close").GetString()!);

        return new Stock
        {
            Symbol = symbol,
            CurrentPrice = closePrice,
            Change = 0,
            PercentChange = 0,
            LastUpdated = DateTime.Parse(timestamp)
        };
    }
}
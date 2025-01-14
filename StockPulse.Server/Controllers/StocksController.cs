using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StockPulse.Server.Models;
using StockPulse.Server.Services;
using System.Text.Json;

namespace StockPulse.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController(AlphaVantageService financeService, IConnectionMultiplexer redis) : ControllerBase
{
    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetStock(string symbol)
    {
        var db = redis.GetDatabase();
        var cachedStock = await db.StringGetAsync(symbol);

        if (!string.IsNullOrEmpty(cachedStock))
        {
            var stock = JsonSerializer.Deserialize<Stock>(cachedStock!);
            return Ok(stock);
        }

        var liveStock = await financeService.GetStockPriceAsync(symbol);
        if (liveStock == null) return NotFound();

        await db.StringSetAsync(symbol, JsonSerializer.Serialize(liveStock), TimeSpan.FromMinutes(5));
        return Ok(liveStock);
    }
}
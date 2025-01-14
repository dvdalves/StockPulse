namespace StockPulse.Server.Models;

public class Stock
{
    public required string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal Change { get; set; }
    public decimal PercentChange { get; set; }
    public DateTime LastUpdated { get; set; }
}

using System;

namespace StockTradingSimulator.Models
{
    // Represents holdings for a symbol
    public class PortfolioItem
    {
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        // Optional stop-loss price
        public decimal? StopLossPrice { get; set; }

        public PortfolioItem() { }

        public PortfolioItem(string symbol, decimal quantity, decimal avgPrice)
        {
            Symbol = symbol;
            Quantity = quantity;
            AveragePrice = avgPrice;
        }
    }
}

using System;

namespace StockTradingSimulator.Models
{
    // Abstract base for traded assets
    public abstract class Asset
    {
        public string Symbol { get; init; }
        public decimal Price { get; protected set; }
        public decimal LastPrice { get; protected set; }

        protected Asset(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
            LastPrice = price;
        }

        // Update price using provided RNG
        public abstract void UpdatePrice(Random rng);

        public bool PriceUp => Price > LastPrice;
    }
}

using System;

namespace StockTradingSimulator.Models
{
    // Crypto assets have higher volatility
    public class Crypto : Asset
    {
        public Crypto(string symbol, decimal price) : base(symbol, price) { }

        public override void UpdatePrice(Random rng)
        {
            LastPrice = Price;
            // Volatility: +/- up to 10% (scaled)
            var pct = (decimal)(rng.NextDouble() * 0.2 - 0.1); // -10% to +10%
            var change = Price * pct;
            Price = Math.Round(Price + change, 2);
            if (Price <= 0) Price = LastPrice * 0.5m; // safety
        }
    }
}

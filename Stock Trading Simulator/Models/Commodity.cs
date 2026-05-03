using System;

namespace StockTradingSimulator.Models
{
    // Commodities are less volatile
    public class Commodity : Asset
    {
        public Commodity(string symbol, decimal price) : base(symbol, price) { }

        public override void UpdatePrice(Random rng)
        {
            LastPrice = Price;
            // Volatility: +/- up to 2.5%
            var pct = (decimal)(rng.NextDouble() * 0.05 - 0.025); // -2.5% to +2.5%
            var change = Price * pct;
            Price = Math.Round(Price + change, 2);
            if (Price <= 0) Price = LastPrice * 0.9m; // safety
        }
    }
}

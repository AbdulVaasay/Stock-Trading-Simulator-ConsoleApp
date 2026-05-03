using System;
using System;
using System.Collections.Generic;
using System.Linq;
using StockTradingSimulator.Models;

namespace StockTradingSimulator.Services
{
    // Maintains market assets and updates their prices
    public class MarketService
    {
        private readonly Random _rng = new();
        private readonly object _sync = new();

        public List<Asset> Assets { get; } = new();

        public MarketService() { }

        public void SeedDefaultMarket()
        {
            // Add a few example assets
            Assets.Clear();
            Assets.Add(new Crypto("BTC", 60000m));
            Assets.Add(new Crypto("ETH", 3500m));
            Assets.Add(new Commodity("GOLD", 1800m));
            Assets.Add(new Commodity("SILVER", 24m));
        }

        public Asset? FindAsset(string symbol)
        {
            lock (_sync)
            {
                return Assets.FirstOrDefault(a => string.Equals(a.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
            }
        }

        // Update all prices (manual trigger)
        public void UpdatePrices()
        {
            lock (_sync)
            {
                foreach (var a in Assets)
                {
                    a.UpdatePrice(_rng);
                }
            }
        }
    }
}

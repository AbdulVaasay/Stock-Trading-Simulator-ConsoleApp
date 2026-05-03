using System;
using System.Collections.Generic;
using System.Linq;
using StockTradingSimulator.Models;
using StockTradingSimulator.Services;

namespace StockTradingSimulator.Strategies
{
    // Random buy/sell strategy for testing
    public class RandomStrategy : ITradingStrategy
    {
        private readonly Random _rng = new();

        public void Execute(Trader trader, List<Asset> market)
        {
            if (market == null || market.Count == 0) return;

            // Pick a random asset
            var asset = market[_rng.Next(market.Count)];
            // Decide action
            var doBuy = _rng.NextDouble() > 0.5;

            if (doBuy)
            {
                // Use a fraction of balance to buy
                var fraction = (decimal)(_rng.NextDouble() * 0.2); // up to 20%
                var amount = trader.Balance * fraction;
                if (amount < asset.Price) return; // can't buy
                var qty = Math.Floor(amount / asset.Price * 100m) / 100m; // 2 decimals
                if (qty <= 0) return;
                trader.Buy(asset.Symbol, qty, asset.Price);
            }
            else
            {
                // Sell a random holding if any
                var holdings = trader.Portfolio.Where(p => p.Symbol == asset.Symbol).FirstOrDefault();
                if (holdings == null) return;
                var maxSell = holdings.Quantity;
                if (maxSell <= 0) return;
                // sell between 10% and 100%
                var pct = (decimal)(_rng.NextDouble() * 0.9 + 0.1);
                var qty = Math.Round(maxSell * pct, 2);
                if (qty <= 0) return;
                trader.Sell(asset.Symbol, qty, asset.Price);
            }
        }
    }
}

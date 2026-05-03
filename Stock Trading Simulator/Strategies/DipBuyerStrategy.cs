using System;
using System.Collections.Generic;
using System.Linq;
using StockTradingSimulator.Models;
using StockTradingSimulator.Services;

namespace StockTradingSimulator.Strategies
{
    // Buys assets when they dip compared to last price
    public class DipBuyerStrategy : ITradingStrategy
    {
        private readonly Random _rng = new();

        public void Execute(Trader trader, List<Asset> market)
        {
            foreach (var asset in market)
            {
                // Consider dips greater than 0.5%
                if (asset.LastPrice <= 0) continue;
                var dropPct = (asset.LastPrice - asset.Price) / asset.LastPrice;
                if (dropPct >= 0.005m)
                {
                    // Buy a modest amount: 1% to 3% of balance
                    var fraction = (decimal)(_rng.NextDouble() * 0.02 + 0.01);
                    var amount = trader.Balance * fraction;
                    if (amount < asset.Price) continue;
                    var qty = Math.Floor(amount / asset.Price * 100m) / 100m;
                    if (qty <= 0) continue;
                    trader.Buy(asset.Symbol, qty, asset.Price);
                }
            }
        }
    }
}

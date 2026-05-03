using System.Collections.Generic;
using StockTradingSimulator.Models;
using StockTradingSimulator.Services;

namespace StockTradingSimulator.Strategies
{
    // Strategy pattern interface
    public interface ITradingStrategy
    {
        void Execute(Trader trader, List<Asset> market);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using StockTradingSimulator.Models;

namespace StockTradingSimulator.Services
{
    // Handles JSON persistence for portfolio and transactions
    public class FileService
    {
        private readonly string _portfolioPath = "portfolio.json";
        private readonly string _transactionsPath = "transactions.json";

        private readonly JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public void SavePortfolio(IEnumerable<PortfolioItem> portfolio)
        {
            var json = JsonSerializer.Serialize(portfolio, _opts);
            File.WriteAllText(_portfolioPath, json);
        }

        public List<PortfolioItem>? LoadPortfolio()
        {
            if (!File.Exists(_portfolioPath)) return null;
            var json = File.ReadAllText(_portfolioPath);
            return JsonSerializer.Deserialize<List<PortfolioItem>>(json, _opts);
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, _opts);
            File.WriteAllText(_transactionsPath, json);
        }

        public List<Transaction>? LoadTransactions()
        {
            if (!File.Exists(_transactionsPath)) return null;
            var json = File.ReadAllText(_transactionsPath);
            return JsonSerializer.Deserialize<List<Transaction>>(json, _opts);
        }
    }
}

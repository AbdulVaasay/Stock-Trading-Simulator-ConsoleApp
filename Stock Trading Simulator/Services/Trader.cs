using System;
using System.Collections.Generic;
using System.Linq;
using StockTradingSimulator.Models;

namespace StockTradingSimulator.Services
{
    // Trader encapsulates balance and portfolio operations
    public class Trader
    {
        private decimal _balance;

        public decimal Balance => _balance;
        public List<PortfolioItem> Portfolio { get; } = new();
        public List<Transaction> Transactions { get; } = new();

        // Notifications are raised instead of writing directly to console to avoid background prints
        public event Action<string>? Notification;

        public Trader()
        {
            _balance = 0m;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit must be positive", nameof(amount));
            _balance += amount;
        }

        // Buy quantity at price
        public void Buy(string symbol, decimal qty, decimal price)
        {
            if (qty <= 0) throw new ArgumentException("Quantity must be positive", nameof(qty));
            if (price <= 0) throw new ArgumentException("Price must be positive", nameof(price));

            var cost = qty * price;
            if (cost > _balance) { Notify("Insufficient balance to buy.", isError: true); return; }

            var item = Portfolio.FirstOrDefault(p => string.Equals(p.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                item = new PortfolioItem(symbol, qty, price);
                Portfolio.Add(item);
            }
            else
            {
                var totalQty = item.Quantity + qty;
                var totalCost = item.AveragePrice * item.Quantity + price * qty;
                item.AveragePrice = totalCost / totalQty;
                item.Quantity = totalQty;
            }

            _balance -= cost;
            Transactions.Add(new Transaction(TransactionType.Buy, symbol, qty, price));
            // Notify success
            Notify($"Bought {qty} {symbol} at {price:C} (cost {cost:C}). Balance: {_balance:C}");
        }

        // Sell quantity at price
        public void Sell(string symbol, decimal qty, decimal price)
        {
            if (qty <= 0) throw new ArgumentException("Quantity must be positive", nameof(qty));
            if (price <= 0) throw new ArgumentException("Price must be positive", nameof(price));

            var item = Portfolio.FirstOrDefault(p => string.Equals(p.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
            if (item == null || item.Quantity < qty) { Notify("Not enough holdings to sell.", isError: true); return; }

            var proceeds = qty * price;
            item.Quantity -= qty;
            if (item.Quantity <= 0)
            {
                Portfolio.Remove(item);
            }

            _balance += proceeds;
            Transactions.Add(new Transaction(TransactionType.Sell, symbol, qty, price));
            Notify($"Sold {qty} {symbol} at {price:C} (proceeds {proceeds:C}). Balance: {_balance:C}");
        }

        public void LoadPortfolio(List<PortfolioItem> items)
        {
            Portfolio.Clear();
            Portfolio.AddRange(items);
        }

        public void LoadTransactions(List<Transaction> tx)
        {
            Transactions.Clear();
            Transactions.AddRange(tx);
        }

        // Evaluate stop losses against current market prices and sell if triggered
        public void EvaluateStopLoss(IEnumerable<Asset> market)
        {
            var toSell = new List<(PortfolioItem item, Asset asset)>();
            foreach (var p in Portfolio.ToList())
            {
                if (!p.StopLossPrice.HasValue) continue;
                var asset = market.FirstOrDefault(a => string.Equals(a.Symbol, p.Symbol, StringComparison.OrdinalIgnoreCase));
                if (asset == null) continue;
                if (asset.Price <= p.StopLossPrice.Value)
                {
                    toSell.Add((p, asset));
                }
            }

            foreach (var (item, asset) in toSell)
            {
                Notify($"Stop-loss: selling {item.Quantity} {item.Symbol} at {asset.Price:C} (stop {item.StopLossPrice:C}).", isError: true);
                Sell(item.Symbol, item.Quantity, asset.Price);
            }
        }

        private void Notify(string message, bool isError = false)
        {
            if (Notification != null)
            {
                Notification.Invoke(message);
            }
            else
            {
                // Fallback to immediate console write if no subscriber
                if (isError) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}

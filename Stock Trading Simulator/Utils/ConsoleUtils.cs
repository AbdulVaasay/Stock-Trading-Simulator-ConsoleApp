using System;
using System;
using System.Collections.Generic;
using System.Linq;
using StockTradingSimulator.Models;
using StockTradingSimulator.Services;

namespace StockTradingSimulator.Utils
{
    public static class ConsoleUtils
    {
        public static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  market            - update prices and show market");
            Console.WriteLine("  prices            - show current prices");
            Console.WriteLine("  buy <sym> <qty>   - buy quantity of symbol");
            Console.WriteLine("  sell <sym> <qty>  - sell quantity of symbol");
            Console.WriteLine("  portfolio         - show portfolio and P/L");
            Console.WriteLine("  history           - show transaction history");
            Console.WriteLine("  strategy <name> [n] - run strategy (random|dip) n times");
            Console.WriteLine("  deposit <amount>  - deposit funds");
            Console.WriteLine("  save              - save portfolio & transactions");
            Console.WriteLine("  load              - load saved data");
            Console.WriteLine("  exit              - save and exit");
        }

        public static void PrintMarket(List<Asset> assets)
        {
            Console.WriteLine("Market:");
            Console.WriteLine("Symbol\tPrice\tChange");
            foreach (var a in assets)
            {
                var arrow = a.Price > a.LastPrice ? "?" : a.Price < a.LastPrice ? "?" : "-";
                var diff = a.Price - a.LastPrice;
                var pct = a.LastPrice == 0 ? 0 : (double)(diff / a.LastPrice * 100m);
                // Color output: green for up, red for down
                if (diff > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (diff < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ResetColor();

                Console.Write($"{a.Symbol}\t{a.Price:C}\t{arrow} {diff:C} ({pct:F2}%)");
                Console.ResetColor();
                Console.WriteLine(a is Crypto ? " [Crypto]" : "");
            }
        }

        public static void PrintPortfolio(Trader trader, List<Asset> market)
        {
            Console.WriteLine($"Balance: {trader.Balance:C}");
            Console.WriteLine("Portfolio:");
            Console.WriteLine("Symbol\tQty\tAvg\tPrice\tUnrealized P/L");
            decimal totalPl = 0m;
            foreach (var p in trader.Portfolio)
            {
                var asset = market.FirstOrDefault(a => string.Equals(a.Symbol, p.Symbol, StringComparison.OrdinalIgnoreCase));
                var current = asset?.Price ?? 0m;
                var pl = (current - p.AveragePrice) * p.Quantity;
                totalPl += pl;

                // Write row with colored P/L
                Console.Write($"{p.Symbol}\t{p.Quantity}\t{p.AveragePrice:C}\t{current:C}\t");
                if (pl > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (pl < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ResetColor();

                Console.WriteLine($"{pl:C}");
                Console.ResetColor();
            }
            // Total unrealized P/L colored
            if (totalPl > 0) Console.ForegroundColor = ConsoleColor.Green;
            else if (totalPl < 0) Console.ForegroundColor = ConsoleColor.Red;
            else Console.ResetColor();
            Console.WriteLine($"Total unrealized P/L: {totalPl:C}");
            Console.ResetColor();
        }

        public static void PrintTransactions(List<Transaction> transactions)
        {
            Console.WriteLine("Transactions:");
            Console.WriteLine("Time (UTC)\tType\tSymbol\tQty\tPrice");
            foreach (var t in transactions.OrderByDescending(x => x.Timestamp))
            {
                // Color buys green, sells red
                if (t.Type == TransactionType.Buy)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (t.Type == TransactionType.Sell)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ResetColor();

                Console.WriteLine($"{t.Timestamp:u}\t{t.Type}\t{t.Symbol}\t{t.Quantity}\t{t.Price:C}");
                Console.ResetColor();
            }
        }
    }
}

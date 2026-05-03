// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using StockTradingSimulator.Models;
using StockTradingSimulator.Services;
using StockTradingSimulator.Strategies;
using StockTradingSimulator.Utils;

namespace StockTradingSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize services
            var market = new MarketService();
            market.SeedDefaultMarket();
            var trader = new Trader();
            var fileService = new FileService();

            // Market updates are manual via the 'market' command

            // Load persisted data if available
            try
            {
                var loadedPortfolio = fileService.LoadPortfolio();
                if (loadedPortfolio != null)
                {
                    trader.LoadPortfolio(loadedPortfolio);
                }

                var loadedTx = fileService.LoadTransactions();
                if (loadedTx != null)
                {
                    trader.LoadTransactions(loadedTx);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: failed to load saved data - {ex.Message}");
            }

            // Provide initial funds for demo purposes
            if (trader.Balance <= 0)
            {
                trader.Deposit(100_000m);
            }

            // Strategies
            var randomStrategy = new RandomStrategy();
            var dipStrategy = new DipBuyerStrategy();

            Console.WriteLine("Stock Trading Simulator - Console UI");
            Console.WriteLine("Type 'help' to see commands.");

            var notifications = new System.Collections.Concurrent.ConcurrentQueue<string>();
            // Subscribe to trader notifications and enqueue them for display at prompt time
            trader.Notification += (msg) => notifications.Enqueue(msg);
            // Market updates are manual; no background notifications

            while (true)
            {
                // Flush notifications before showing prompt so user sees updates when ready
                while (notifications.TryDequeue(out var note))
                {
                    if (string.IsNullOrWhiteSpace(note)) continue;
                    if (note.StartsWith("Insufficient", StringComparison.OrdinalIgnoreCase) || note.StartsWith("Not enough", StringComparison.OrdinalIgnoreCase) || note.StartsWith("Stop-loss", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(note);
                        Console.ResetColor();
                    }
                    else if (note.StartsWith("Bought", StringComparison.OrdinalIgnoreCase) || note.StartsWith("Sold", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(note);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(note);
                    }
                }

                Console.WriteLine();
                Console.Write("cmd> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                try
                {
                    switch (cmd)
                    {
                        case "help":
                            ConsoleUtils.PrintHelp();
                            break;

                        case "market":
                            market.UpdatePrices();
                            trader.EvaluateStopLoss(market.Assets);
                            ConsoleUtils.PrintMarket(market.Assets);
                            break;

                        case "prices":
                            ConsoleUtils.PrintMarket(market.Assets);
                            break;

                        case "buy":
                            if (parts.Length < 3) { Console.WriteLine("Usage: buy <symbol> <qty>"); break; }
                            var bsym = parts[1].ToUpperInvariant();
                            if (!decimal.TryParse(parts[2], out var bqty) || bqty <= 0) { Console.WriteLine("Invalid quantity"); break; }
                            var basset = market.FindAsset(bsym);
                            if (basset == null) { Console.WriteLine("Unknown symbol"); break; }
                            trader.Buy(basset.Symbol, bqty, basset.Price);
                            break;

                        case "sell":
                            if (parts.Length < 3) { Console.WriteLine("Usage: sell <symbol> <qty>"); break; }
                            var ssym = parts[1].ToUpperInvariant();
                            if (!decimal.TryParse(parts[2], out var sqty) || sqty <= 0) { Console.WriteLine("Invalid quantity"); break; }
                            var sasset = market.FindAsset(ssym);
                            if (sasset == null) { Console.WriteLine("Unknown symbol"); break; }
                            trader.Sell(sasset.Symbol, sqty, sasset.Price);
                            break;

                        case "portfolio":
                            ConsoleUtils.PrintPortfolio(trader, market.Assets);
                            break;

                        case "history":
                            ConsoleUtils.PrintTransactions(trader.Transactions);
                            break;

                        case "deposit":
                            if (parts.Length < 2 || !decimal.TryParse(parts[1], out var amt) || amt <= 0) { Console.WriteLine("Usage: deposit <amount>"); break; }
                            trader.Deposit(amt);
                            Console.WriteLine($"Deposited {amt:C}. New balance: {trader.Balance:C}");
                            break;

                        case "save":
                            fileService.SavePortfolio(trader.Portfolio);
                            fileService.SaveTransactions(trader.Transactions);
                            Console.WriteLine("Saved portfolio and transactions.");
                            break;

                        case "load":
                            var lp = fileService.LoadPortfolio();
                            if (lp != null) { trader.LoadPortfolio(lp); Console.WriteLine("Loaded portfolio."); }
                            var lt = fileService.LoadTransactions();
                            if (lt != null) { trader.LoadTransactions(lt); Console.WriteLine("Loaded transactions."); }
                            break;

                        case "strategy":
                            if (parts.Length < 2) { Console.WriteLine("Usage: strategy <random|dip> [iterations]"); break; }
                            var strat = parts[1].ToLowerInvariant();
                            if (!int.TryParse(parts.Length >= 3 ? parts[2] : "1", out var iter) || iter <= 0) iter = 1;
                            for (int i = 0; i < iter; i++)
                            {
                                market.UpdatePrices();
                                if (strat == "random") randomStrategy.Execute(trader, market.Assets);
                                else if (strat == "dip") dipStrategy.Execute(trader, market.Assets);
                                trader.EvaluateStopLoss(market.Assets);
                            }
                            Console.WriteLine("Strategy run complete.");
                            break;

                        case "exit":
                        case "quit":
                            fileService.SavePortfolio(trader.Portfolio);
                            fileService.SaveTransactions(trader.Transactions);
                            return;

                        default:
                            Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

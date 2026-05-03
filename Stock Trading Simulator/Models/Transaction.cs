using System;

namespace StockTradingSimulator.Models
{
    public enum TransactionType { Buy, Sell }

    public class Transaction
    {
        public TransactionType Type { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction() { }

        public Transaction(TransactionType type, string symbol, decimal qty, decimal price)
        {
            Type = type;
            Symbol = symbol;
            Quantity = qty;
            Price = price;
            Timestamp = DateTime.UtcNow;
        }
    }
}

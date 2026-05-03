📈 Stock Trading Simulator (C# Console App)

A console-based stock trading simulator built in C# using OOP principles. This project mimics a simplified financial market where users can trade assets, manage a portfolio, and test basic trading strategies.

🚀 Features
📊 Simulated market with dynamic price updates
💰 Buy and sell assets (stocks/crypto)
📁 Portfolio tracking with real-time P/L
📜 Transaction history logging
🧠 Automated trading strategies:
Random Strategy
Dip Buyer Strategy
💾 Save & load portfolio data
🎨 Colored console output (profit/loss visualization)
🧱 Project Structure
Stock Trading Simulator
│
├── Models
│   ├── Asset.cs
│   ├── Commodity.cs
│   ├── Crypto.cs
│   ├── PortfolioItem.cs
│   └── Transaction.cs
│
├── Services
│   ├── FileService.cs
│   ├── MarketService.cs
│   └── Trader.cs
│
├── Strategies
│   ├── ITradingStrategy.cs
│   ├── RandomStrategy.cs
│   └── DipBuyerStrategy.cs
│
├── Utils
│   └── ConsoleUtils.cs
│
├── Program.cs
└── Dockerfile
🧠 OOP Concepts Used

This project demonstrates core Object-Oriented Programming concepts:

Encapsulation → Trader, PortfolioItem, Transaction
Inheritance → Asset → Crypto, Commodity
Polymorphism → ITradingStrategy interface with multiple implementations
Abstraction → Service layer (MarketService, FileService)
⚙️ How It Works
The market updates asset prices dynamically.
The trader can execute buy/sell operations.
Portfolio updates automatically with average price and P/L.
Strategies can simulate automated trading behavior.
Data can be persisted using file storage.
🖥️ Available Commands
market              - update prices and show market
prices              - show current prices
buy <sym> <qty>     - buy quantity of symbol
sell <sym> <qty>    - sell quantity of symbol
portfolio           - show portfolio and P/L
history             - show transaction history
strategy <name> [n] - run strategy (random|dip) n times
deposit <amount>    - deposit funds
save                - save portfolio & transactions
load                - load saved data
exit                - save and exit
📦 Requirements
.NET SDK (6.0 or later recommended)
Visual Studio 2022 (or any C# IDE)
▶️ How to Run
Option 1: Using Visual Studio
Open the solution in Visual Studio 2022
Set project as Startup Project
Press F5 or click Run
Option 2: Using CLI
dotnet build
dotnet run
🐳 Docker Support (Optional)

If your Dockerfile is configured:

docker build -t stock-sim .
docker run -it stock-sim
📊 Example Output
Market:
Symbol   Price     Change
BTC      $42000    ↑ +500 (1.20%)
AAPL     $180      ↓ -2 (-1.10%)

Portfolio:
Balance: $10,000
Symbol   Qty   Avg     Price   P/L
BTC      1     40000   42000   +2000
⚠️ Limitations
No real market data (fully simulated)
Basic strategies (not profitable in real trading)
No risk management or advanced indicators
🔮 Future Improvements
Add technical indicators (RSI, EMA, Bollinger Bands)
Implement real-time API (e.g., Binance)
GUI version (WPF or WinForms)
Advanced strategies (AI/ML-based)
Multi-user support
📜 License

This project is for educational purposes only.

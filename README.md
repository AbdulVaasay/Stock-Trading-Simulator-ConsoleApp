# Stock Trading Simulator (C# Console App)

A console-based stock trading simulator built in C# using Object-Oriented Programming principles.  
It simulates a financial market where users can trade assets, manage a portfolio, and test trading strategies.

---

## Features

- Simulated market with dynamic price updates  
- Buy and sell assets (stocks/crypto)  
- Portfolio tracking with real-time profit/loss  
- Transaction history logging  
- Automated trading strategies:
  - Random Strategy  
  - Dip Buyer Strategy  
- Save and load portfolio data  
- Colored console output (profit/loss visualization)

---

## Project Structure
StockTradingSimulator/
│
├── Models/
│ ├── Asset.cs
│ ├── Commodity.cs
│ ├── Crypto.cs
│ ├── PortfolioItem.cs
│ └── Transaction.cs
│
├── Services/
│ ├── FileService.cs
│ ├── MarketService.cs
│ └── Trader.cs
│
├── Strategies/
│ ├── ITradingStrategy.cs
│ ├── RandomStrategy.cs
│ └── DipBuyerStrategy.cs
│
├── Utils/
│ └── ConsoleUtils.cs
│
├── Program.cs
└── Dockerfile

---

## OOP Concepts Used

- **Encapsulation** → Trader, PortfolioItem, Transaction  
- **Inheritance** → Asset → Crypto, Commodity  
- **Polymorphism** → ITradingStrategy interface  
- **Abstraction** → Service layer (MarketService, FileService)

---

## How It Works

- Market prices update dynamically  
- User can buy/sell assets  
- Portfolio updates with average price and P/L  
- Strategies simulate automated trading  
- Data can be saved and loaded from storage  

---

## Available Commands
- market - update prices and show market
- prices - show current prices
- buy <sym> <qty> - buy quantity of symbol
- sell <sym> <qty> - sell quantity of symbol
- portfolio - show portfolio and P/L
- history - show transaction history
- strategy <name> [n] - run strategy (random|dip) n times
- deposit <amount> - deposit funds
- save - save portfolio & transactions
- load - load saved data
- exit - save and exit

---

## Requirements

- .NET SDK 6.0 or later  
- Visual Studio 2022 (or any C# IDE)

---

## How to Run

### Using Visual Studio
1. Open project in Visual Studio  
2. Set as Startup Project  
3. Press F5  

### Using CLI
- dotnet build
- dotnet run

### Using Docker
- docker run --rm -it stock-sim

---

## Notes

This project uses simulated data and is intended for educational purposes only.

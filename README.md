# Strzon Binance Trading Bot

A binance connected trading bot if the price of any coin in the portfolio dropped below the fixed price, then the bot sells to
USDT wallet. If risen above fixed price then bot buys that coin or token again. 

## Installation

Close with HTTPS: 
```bash
git clone https://github.com/ogu83/StrzonBinanceTradingBot.git
cd StrzonBinanceTradingBot
```

### The .NET Core installation
Please install dotnet core if it is not installed already.
https://dotnet.microsoft.com/en-us/download
Here is the setup for all OS.

### Migrations

If not installed please install dotnet-ef to apply migrations over db.
```bash
dotnet tool install --global dotnet-ef
```

Edit the default connection string at [appSettings.json](appSettings.json). The default is *Server=127.0.0.1;Database=KopiorQuesta-Marketplace;User Id=sa;Password=Sa12345678*;*

### Sqllite
Run this command to apply database schema and seed data to database.

```bash
dotnet ef database update -v 
```

### Development HTTPS Certificate

Trust the HTTPS development certificate by running the following command:

```bash
dotnet dev-certs https --trust
```

### Run
```bash
dotnet restore
dotnet build
dotnet run
```
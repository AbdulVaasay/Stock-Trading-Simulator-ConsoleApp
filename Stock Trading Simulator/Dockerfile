FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all files and restore
COPY . .
RUN dotnet restore "Stock Trading Simulator/Stock Trading Simulator.csproj"

# Publish
RUN dotnet publish "Stock Trading Simulator/Stock Trading Simulator.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Stock Trading Simulator.dll"]

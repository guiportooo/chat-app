FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ChatApp.StockBot.csproj", "./"]
RUN dotnet restore "ChatApp.StockBot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ChatApp.StockBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatApp.StockBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait
CMD /wait && dotnet ChatApp.StockBot.dll
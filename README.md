# chat-app

[The Challenge](./docs/TheChallenge.md)

## Architecture

![Architecture](/docs/Architecture.png)

### Sending simple messages to the chat

1. Chat App sends message to .NET API (Ex.: `Hi, there`)
2. Chat API publishes message to SignalR HUB
3. Chat API saves the message to the SQL Server database
4. All connected Chat Apps consume the message from the SignalR HUB and display the new message to the user

### Sending `/stock` command messages to the chat

1. Chat App sends message to .NET API (Ex.: `/stock=APPL.US`)
2. Chat API publishes message to SignalR HUB
3. Chat API publishes command to RabbitMQ Broker
4. All connected Chat Apps consume the message from the SignalR HUB and display the new message to the user
5. Stock Bot consumes the command from the RabbitMQ Broker
6. Stock Bot calls Stooq API to get the quote for the stock
7. Stock Bot publishes stock quote to RabbitMQ Broker
8. Chat API consumes the stock quote from the RabbitMQ Broker
9. Chat API publishes stock quote to SignalR HUB
10. All connected Chat Apps consume the message from the SignalR HUB and display the stock quote to the user

## Considerations

There is a problem when getting the information for the stock from the Stooq API, where the CSV content is being retrieved by the Stock Bot as follows:

```
Symbol,Date,Time,Open,High,Low,Close,Volume
APPL.US,N/D,N/D,N/D,N/D,N/D,N/D,N/D
```

So, for this reason, the Stock Bot response is being displayed as:

“APPL.US quote is $N/D per share”

:disappointed_relieved:

## Dependencies

- [Docker](https://www.docker.com)
- [.NET SDK 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Node.js 14](https://nodejs.org/en/)
- [NPM](https://www.npmjs.com)
- [Yarn (optional)](https://yarnpkg.com)

## Running on Docker

### Configure dev certificate for Chat API HTTPS redirection

Change the certificate password on the **docker-compose.yaml** file to match you local dev certificate's password:

The password can be configured replacing `[your_password_here]` with your password on the **line 21** of the docker-compose.yaml:

```
ASPNETCORE_Kestrel__Certificates__Default__Password=[your_password_here]
```

If you don't know your local dev certificate's password, you can generate a new one running:

```bash
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p [your_password_here]
dotnet dev-certs https --trust
```

### Run with Docker Compose

1. Run the command `docker-compose build`
2. Run the command `docker-compose up`
3. The Chat API will be available at _https://localhost:5001/swagger/index.html_
4. The Chat App will be available at _http://localhost:3000_
5. The RabbitMQ management dashboard will be available at _http://localhost:15672_ (`user: guest`, `password: guest`)

## Running locally

### Running SQLServer on Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=local@db_123" -p 1433:1433 --name sql-server -h sql-server -d mcr.microsoft.com/mssql/server:2019-latest
```

### Running RabbitMQ on Docker

```bash
docker run -d --hostname rabbitmq --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

The RabbitMQ management dashboard will be available at _http://localhost:15672_ (`user: guest`, `password: guest`)

### Running the Stock Bot

1. Navigate to /backend/src/Chat.StockBot
2. Run the command `dotnet build`
3. Run the command `dotnet run`

### Running the API

1. Navigate to /backend/src/Chat.Api
2. Run the command `dotnet build`
3. Run the command `dotnet run`
4. The Chat API will be available at _https://localhost:5001/swagger/index.html_

### Running the App

1. Navigate to /frontend/chat-app
2. Run the command `yarn install` or `npm install`
3. Run the command `yarn start` or `npm start`
4. The Chat App will be available at _http://localhost:3000_

## Running the tests

1. Navigate to /backend
2. Run the command `dotnet build`
3. Run the command `dotnet test`

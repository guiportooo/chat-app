# chat-app

## .Net Challenge

### Description

This project is designed to test your knowledge of back-end web technologies, specifically in
.NET and assess your ability to create back-end products with attention to details, standards,
and reusability.

### Assignment

The goal of this exercise is to create a simple browser-based chat application using .NET.
This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

### Mandatory Features

- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
  /stock=stock_code
- Create a decoupled bot that will call an API using the stock_code as a parameter
  (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
  stock_code)
- The bot should parse the received CSV file and then it should send a message back into
  the chatroom using a message broker like RabbitMQ. The message will be a stock quote
  using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
  the bot.
- Have the chat messages ordered by their timestamps and show only the last 50
  messages.
- Unit test the functionality you prefer.

### Bonus (Optional)

- Have more than one chatroom.
- Use .NET identity for users authentication
- Handle messages that are not understood or any exceptions raised within the bot.
- Build an installer.

### Considerations

- We will open 2 browser windows and log in with 2 different users to test the
  functionalities.
- The stock command won’t be saved on the database as a post.
- The project is totally focused on the backend; please have the frontend as simple as you
  can.
- Keep confidential information secure.
- Pay attention if your chat is consuming too many resources.
- Keep your code versioned with Git locally.
- Feel free to use small helper libraries

### Deliverables

When you finish the assignment, send a zip file (don’t forget to include the .git/ folder.) or upload
your project to your Git repo (Github, BitBucket, etc...) and share the repository link with your
initial contact via email. Indicate which, if any, of the bonus tasks you completed.
If you didn’t manage to finish everything, please tell us which parts you completed.

# Solution

## Architecture

![Architecture](/docs/Architecture.png)

### Sending simple messages to the chat

1. Chat App sends message to .NET API (Ex.)
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

## Observations

There is a problem when getting the information for the stock from the Stooq API, where the CSV content is being retrieved by the Stock Bot as follows:

```
Symbol,Date,Time,Open,High,Low,Close,Volume<br>
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

## Running locally

### Running SQLServer with Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=local@db_123" -p 1433:1433 --name sql-server -h sql-server -d mcr.microsoft.com/mssql/server:2019-latest
```

### Running RabbitMQ with Docker

```bash
docker run -d --hostname localhost --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

### Running the API

1. Navigate to /backend/src/Chat.Api
2. Run the command `dotnet build`
3. Run the command `dotnet run`

### Running the Stock Bot

1. Navigate to /backend/src/Chat.StockBot
2. Run the command `dotnet build`
3. Run the command `dotnet run`

### Running the App

1. Navigate to /frontend/chat-app
2. Run the command `yarn install` or `npm install`
3. Run the command `yarn start` or `npm start`
4. Open the browser and navigate to http://localhost:3000

## Running the tests

1. Navigate to /backend
2. Run the command `dotnet build`
3. Run the command `dotnet test`

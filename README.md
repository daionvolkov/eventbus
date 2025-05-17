# EventBus (.NET) — Minimal TCP Event Bus in Docker

A minimal, educational event bus implemented in .NET (C#) for learning network programming and microservice communications.  
Supports event broadcasting between multiple clients via a simple TCP protocol using JSON events.

## Features

- Pure .NET (C#) implementation — no external dependencies
- TCP-based server with asynchronous client handling
- Structured event messages (with type, data, sender, timestamp)
- Works with any number of clients (in Docker containers or locally)
- Ready for extension: add subscriptions, filtering, logging, authentication, etc.
- Easy to launch with Docker and Docker Compose

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (for containerization)
- [Git](https://git-scm.com/)

### Local Run

1. **Build and run the server:**
    ```bash
    cd EventBusServer
    dotnet run
    ```

2. **Build and run a client (in another terminal):**
    ```bash
    cd EventBusClient
    dotnet run
    ```
   Repeat to launch as many clients as you want.

### Docker Compose

To launch the server and multiple clients in containers:

```bash
docker compose up --build

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY ./publish_server/ .
ENTRYPOINT ["dotnet", "EventBusServer.dll"]

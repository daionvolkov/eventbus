FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY ./publish_client/ .
ENTRYPOINT ["dotnet", "EventBusClient.dll"]

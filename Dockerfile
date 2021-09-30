FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
EXPOSE 80

COPY /app/publish/kamafi.liability.core /app/
ENTRYPOINT ["dotnet", "kamafi.liability.core.dll"]
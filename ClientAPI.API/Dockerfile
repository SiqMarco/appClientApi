FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["ClientAPI.API/ClientAPI.API.csproj", "ClientAPI.API/"]
COPY ["ClientAPI.Application/ClientAPI.Application.csproj", "ClientAPI.Application/"]
COPY ["ClientAPI.Domain/ClientAPI.Domain.csproj", "ClientAPI.Domain/"]
COPY ["ClientAPI.Infrastructure/ClientAPI.Infrastructure.csproj", "ClientAPI.Infrastructure/"]
RUN dotnet restore "ClientAPI.API/ClientAPI.API.csproj"
COPY . .
WORKDIR "/src/ClientAPI.API"
RUN dotnet build "ClientAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClientAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientAPI.API.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk add --no-cache netcat-openbsd
WORKDIR /app
COPY . .
ENTRYPOINT ["./entrypoint.sh"]






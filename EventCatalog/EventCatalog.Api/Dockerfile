# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Music/Eventify/EventCatalog/EventCatalog.Api/EventCatalog.Api.csproj", "Music/Eventify/EventCatalog/EventCatalog.Api/"]
COPY ["Music/Eventify/EventCatalog/EventCatalog.Domain/EventCatalog.Domain.csproj", "Music/Eventify/EventCatalog/EventCatalog.Domain/"]
COPY ["Music/Eventify/Common/Common.Domain/Common.Domain.csproj", "Music/Eventify/Common/Common.Domain/"]
RUN dotnet restore "./Music/Eventify/EventCatalog/EventCatalog.Api/EventCatalog.Api.csproj"
COPY . .
WORKDIR "/src/Music/Eventify/EventCatalog/EventCatalog.Api"
RUN dotnet build "./EventCatalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventCatalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventCatalog.Api.dll"]
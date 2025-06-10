FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Focus.API/Focus.API.csproj", "Focus.API/"]
COPY ["Focus.Infra/Focus.Infra.csproj", "Focus.Infra/"]
COPY ["Focus.Domain/Focus.Domain.csproj", "Focus.Domain/"]
COPY ["Focus.Application/Focus.Application.csproj", "Focus.Application/"]
RUN dotnet restore "Focus.API/Focus.API.csproj"
COPY . .
WORKDIR "/src/Focus.API"
RUN dotnet build "./Focus.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Focus.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Focus.API.dll"]

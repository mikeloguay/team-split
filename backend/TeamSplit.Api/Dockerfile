FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TeamSplit.Api/TeamSplit.Api.csproj", "TeamSplit.Api/"]
RUN dotnet restore "./TeamSplit.Api/TeamSplit.Api.csproj"
COPY . .
WORKDIR "/src/TeamSplit.Api"
RUN dotnet build "./TeamSplit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TeamSplit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamSplit.Api.dll"]
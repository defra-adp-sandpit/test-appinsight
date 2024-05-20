FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get upgrade -y
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update && apt-get upgrade -y
WORKDIR /src
COPY ["./TEST/TEST.csproj", "./TEST/"]
COPY ["./ADP.Portal.Core/ADP.Portal.Core.csproj", "./ADP.Portal.Core/"]
RUN dotnet restore "./TEST/TEST.csproj"
COPY . .
WORKDIR "/src/TEST"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./TEST.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TEST.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TEST.dll"]
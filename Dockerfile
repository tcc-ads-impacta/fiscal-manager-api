FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FiscalManager.sln ./
COPY FiscalManager.Api/FiscalManager.Api.csproj FiscalManager.Api/
COPY FiscalManager.Application/FiscalManager.Application.csproj FiscalManager.Application/
COPY FiscalManager.Domain/FiscalManager.Domain.csproj FiscalManager.Domain/
COPY FiscalManager.Infrastructure/FiscalManager.Infrastructure.csproj FiscalManager.Infrastructure/
RUN dotnet restore FiscalManager.Api/FiscalManager.Api.csproj

COPY . .
RUN dotnet publish FiscalManager.Api/FiscalManager.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FiscalManager.Api.dll"]
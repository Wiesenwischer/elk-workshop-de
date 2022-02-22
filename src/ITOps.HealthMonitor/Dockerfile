#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ITOps.HealthMonitor/ITOps.HealthMonitor.csproj", "ITOps.HealthMonitor/"]
RUN dotnet restore "ITOps.HealthMonitor/ITOps.HealthMonitor.csproj"
COPY . .
WORKDIR "/src/ITOps.HealthMonitor"
RUN dotnet build "ITOps.HealthMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITOps.HealthMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["ITOps.HealthMonitor/corporate.css", "."]
ENTRYPOINT ["dotnet", "ITOps.HealthMonitor.dll"]
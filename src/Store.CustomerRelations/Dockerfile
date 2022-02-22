#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Store.CustomerRelations/Store.CustomerRelations.csproj", "Store.CustomerRelations/"]
COPY ["Store.Shared/Store.Shared.csproj", "Store.Shared/"]
COPY ["Store.Messages/Store.Messages.csproj", "Store.Messages/"]
RUN dotnet restore "Store.CustomerRelations/Store.CustomerRelations.csproj"
COPY . .
WORKDIR "/src/Store.CustomerRelations"
RUN dotnet build "Store.CustomerRelations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Store.CustomerRelations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.CustomerRelations.dll"]
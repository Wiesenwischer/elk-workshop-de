#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Store.ContentManagement/Store.ContentManagement.csproj", "Store.ContentManagement/"]
COPY ["Store.Shared/Store.Shared.csproj", "Store.Shared/"]
COPY ["Store.Messages/Store.Messages.csproj", "Store.Messages/"]
RUN dotnet restore "Store.ContentManagement/Store.ContentManagement.csproj"
COPY . .
WORKDIR "/src/Store.ContentManagement"
RUN dotnet build "Store.ContentManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Store.ContentManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.ContentManagement.dll"]
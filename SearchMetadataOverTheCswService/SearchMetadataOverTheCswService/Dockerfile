#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SearchMetadataOverTheCswService/SearchMetadataOverTheCswService.csproj", "SearchMetadataOverTheCswService/"]
RUN dotnet restore "SearchMetadataOverTheCswService/SearchMetadataOverTheCswService.csproj"
COPY . .
WORKDIR "/src/SearchMetadataOverTheCswService"
RUN dotnet build "SearchMetadataOverTheCswService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchMetadataOverTheCswService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchMetadataOverTheCswService.dll"]
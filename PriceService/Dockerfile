#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PriceService/PriceService.csproj", "PriceService/"]
RUN dotnet restore "PriceService/PriceService.csproj"
COPY . .
WORKDIR "/src/PriceService"
RUN dotnet build "PriceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceService.dll"]
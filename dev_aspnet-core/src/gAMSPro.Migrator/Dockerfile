#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/gAMSPro.Migrator/gAMSPro.Migrator.csproj", "src/gAMSPro.Migrator/"]
COPY ["src/gAMSPro.EntityFrameworkCore/gAMSPro.EntityFrameworkCore.csproj", "src/gAMSPro.EntityFrameworkCore/"]
COPY ["src/gAMSPro.Core/gAMSPro.Core.csproj", "src/gAMSPro.Core/"]
COPY ["src/gAMSPro.Core.Shared/gAMSPro.Core.Shared.csproj", "src/gAMSPro.Core.Shared/"]
RUN dotnet restore "src/gAMSPro.Migrator/gAMSPro.Migrator.csproj"
COPY . .
WORKDIR "/src/src/gAMSPro.Migrator"
RUN dotnet build "gAMSPro.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gAMSPro.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gAMSPro.Migrator.dll", "-s"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/gAMSPro.Web.Public/gAMSPro.Web.Public.csproj", "src/gAMSPro.Web.Public/"]
COPY ["src/gAMSPro.Web.Core/gAMSPro.Web.Core.csproj", "src/gAMSPro.Web.Core/"]
COPY ["src/gAMSPro.Application/gAMSPro.Application.csproj", "src/gAMSPro.Application/"]
COPY ["src/gAMSPro.Application.Shared/gAMSPro.Application.Shared.csproj", "src/gAMSPro.Application.Shared/"]
COPY ["src/gAMSPro.Core.Shared/gAMSPro.Core.Shared.csproj", "src/gAMSPro.Core.Shared/"]
COPY ["src/gAMSPro.Core/gAMSPro.Core.csproj", "src/gAMSPro.Core/"]
COPY ["src/gAMSPro.EntityFrameworkCore/gAMSPro.EntityFrameworkCore.csproj", "src/gAMSPro.EntityFrameworkCore/"]
COPY ["src/gAMSPro.GraphQL/gAMSPro.GraphQL.csproj", "src/gAMSPro.GraphQL/"]
RUN dotnet restore "src/gAMSPro.Web.Public/gAMSPro.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/gAMSPro.Web.Public"
RUN dotnet build "gAMSPro.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gAMSPro.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gAMSPro.Web.Public.dll"]

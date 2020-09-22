#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ILBLI.UCore.TestWeb/ILBLI.UCore.TestWeb.csproj", "ILBLI.UCore.TestWeb/"]
RUN dotnet restore "ILBLI.UCore.TestWeb/ILBLI.UCore.TestWeb.csproj"
COPY . .
WORKDIR "/src/ILBLI.UCore.TestWeb"
RUN dotnet build "ILBLI.UCore.TestWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ILBLI.UCore.TestWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ILBLI.UCore.TestWeb.dll"]
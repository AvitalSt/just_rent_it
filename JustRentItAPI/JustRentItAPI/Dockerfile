#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["JustRentItAPI/JustRentItAPI.csproj", "JustRentItAPI/"]
RUN dotnet restore "JustRentItAPI/JustRentItAPI.csproj"

COPY . .
WORKDIR "/src/JustRentItAPI"
RUN dotnet publish "JustRentItAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

USER root
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgconf-2-4 \
    libatk1.0-0 \
    libgobject-2.0-0 \
    libnss3 \
    libxss1 \
    libasound2 \
    libatk-bridge2.0-0 \
    libgtk-3-0 \
    libgbm1 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "JustRentItAPI.dll"]

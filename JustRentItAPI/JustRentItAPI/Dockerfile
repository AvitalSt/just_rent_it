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
    chromium \
    libnss3 \
    libdbus-1-3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libgbm1 \
    libasound2 \
    libpangocairo-1.0-0 \
    libgtk-3-0 \
    fonts-noto \
    fonts-noto-core \
    fonts-noto-extra \
    fonts-freefont-ttf \
    locales && \
    sed -i 's/# he_IL.UTF-8 UTF-8/he_IL.UTF-8 UTF-8/' /etc/locale.gen && \
    locale-gen && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV LANG=he_IL.UTF-8
ENV LANGUAGE=he_IL:he
ENV LC_ALL=he_IL.UTF-8

ENV PUPPETEER_EXECUTABLE_PATH=/usr/bin/chromium
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "JustRentItAPI.dll"]

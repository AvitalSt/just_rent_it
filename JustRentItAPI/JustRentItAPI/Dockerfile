#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgdiplus \
    fonts-noto-core \
    fonts-freefont-ttf \
    locales && \
    sed -i 's/# he_IL.UTF-8 UTF-8/he_IL.UTF-8 UTF-8/' /etc/locale.gen && \
    locale-gen && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV LANG=he_IL.UTF-8
ENV LANGUAGE=he_IL:he
ENV LC_ALL=he_IL.UTF-8

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["JustRentItAPI/JustRentItAPI.csproj", "JustRentItAPI/"]
RUN dotnet restore "JustRentItAPI/JustRentItAPI.csproj"

COPY . .
WORKDIR "/src/JustRentItAPI"
RUN dotnet publish "JustRentItAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "JustRentItAPI.dll"]

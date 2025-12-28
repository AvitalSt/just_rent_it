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

ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "JustRentItAPI.dll"]

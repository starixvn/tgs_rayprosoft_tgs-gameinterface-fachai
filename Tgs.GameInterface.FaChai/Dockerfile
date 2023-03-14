#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.FaChai/Tgs.GameInterface.FaChai.csproj", "Tgs.GameInterface.FaChai/"]
COPY ["Tgs.GameInterface.FaChai.Core/Tgs.GameInterface.FaChai.Core.csproj", "Tgs.GameInterface.FaChai.Core/"]
RUN dotnet restore "Tgs.GameInterface.FaChai/Tgs.GameInterface.FaChai.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.FaChai"
RUN dotnet build "Tgs.GameInterface.FaChai.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.FaChai.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.FaChai.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Profiles.API/Profiles.API.csproj", "src/Profiles.API/"]
COPY ["src/Profiles.Contracts/Profiles.Contracts.csproj", "src/Profiles.Contracts/"]
COPY ["src/Profiles.Infrastructure/Profiles.Infrastructure.csproj", "src/Profiles.Infrastructure/"]
COPY ["src/Profiles.Domain/Profiles.Domain.csproj", "src/Profiles.Domain/"]
COPY ["src/Profiles.Services.Abstractions/Profiles.Services.Abstractions.csproj", "src/Profiles.Services.Abstractions/"]
COPY ["src/Profiles.Presentation/Profiles.Presentation.csproj", "src/Profiles.Presentation/"]
COPY ["src/Profiles.Services/Profiles.Services.csproj", "src/Profiles.Services/"]
RUN dotnet restore "./src/Profiles.API/Profiles.API.csproj"
COPY . .
WORKDIR "/src/src/Profiles.API"
RUN dotnet build "./Profiles.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Profiles.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profiles.API.dll"]
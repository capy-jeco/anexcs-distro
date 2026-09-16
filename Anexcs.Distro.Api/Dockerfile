FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Anexcs.Distro.Api/Anexcs.Distro.Api.csproj", "Anexcs.Distro.Api/"]
COPY ["Anexcs.Distro.Application/Anexcs.Distro.Application.csproj", "Anexcs.Distro.Application/"]
COPY ["Anexcs.Distro.Domain/Anexcs.Distro.Domain.csproj", "Anexcs.Distro.Domain/"]
COPY ["Anexcs.Distro.Infrastructure/Anexcs.Distro.Infrastructure.csproj", "Anexcs.Distro.Infrastructure/"]
RUN dotnet restore "Anexcs.Distro.Api/Anexcs.Distro.Api.csproj"
COPY . .
WORKDIR "/src/Anexcs.Distro.Api"
RUN dotnet build "./Anexcs.Distro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Anexcs.Distro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Anexcs.Distro.Api.dll"]

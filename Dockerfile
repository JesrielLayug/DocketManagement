# base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# build
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Docket.Server/Docket.Server.csproj", "Docket.Server/"]
COPY ["Docket.Shared/Docket.Shared.csproj", "Docket.Shared/"]

RUN dotnet restore "Docket.Server/Docket.Server.csproj" --disable-parallel
COPY . .
WORKDIR "/src/Docket.Server"
RUN dotnet build "Docket.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish
FROM build AS publish 
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Docket.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Docket.Server.dll"]
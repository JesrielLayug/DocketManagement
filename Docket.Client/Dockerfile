# Use the official Blazor WebAssembly image as a base
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 44398

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY Docket.Client/Docket.Client.csproj Docket.Client/
RUN dotnet restore Docket.Client/Docket.Client.csproj
COPY . .
WORKDIR /src/Docket.Client
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Docket.Client.dll"]
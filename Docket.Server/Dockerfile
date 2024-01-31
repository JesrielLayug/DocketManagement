# Use the official ASP.NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY . .

RUN dotnet restore "./Docket.Server.csproj" --disable-parallel
RUN dotnet publish "./Docket.Server.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 44331

ENTRYPOINT ["dotnet", "DocketServer.dll"]
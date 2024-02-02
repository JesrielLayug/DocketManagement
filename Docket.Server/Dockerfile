# Use the official ASP.NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY . /src

# Restore the dependencies for both projects
RUN dotnet restore "/src/Docket.Server/Docket.Server.csproj" --disable-parallel
RUN dotnet restore "/src/Docket.Shared/Docket.Shared.csproj" --disable-parallel

RUN dotnet publish "/src/Docket.Server/Docket.Server.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Docket.Server.dll"]

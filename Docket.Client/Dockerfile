# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

COPY . /src

RUN dotnet restore "/src/Docket.Client/Docket.Client.csproj" --disable-parallel
RUN dotnet restore "/src/Docket.Shared/Docket.Shared.csproj" --disable-parallel

RUN dotnet publish "/src/Docket.Client/Docket.Client.csproj" -c release -o /app --no-restore

# Final stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the static files from the publish stage
COPY --from=build /app/publish/wwwroot .

# Copy your nginx.conf to the appropriate location
COPY nginx.conf /etc/nginx/nginx.conf
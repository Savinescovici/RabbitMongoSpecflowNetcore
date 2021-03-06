FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY publisher_api/*.csproj ./publisher_api/
COPY publisher_api.Specs/*.csproj ./publisher_api.Specs/
COPY rabbit/*.csproj ./rabbit/
COPY consumer_api/*.csproj ./consumer_api/
RUN dotnet restore

# Copy everything else and build
COPY . ./

WORKDIR /app/consumer_api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

WORKDIR /app

COPY --from=build /app/consumer_api/out .

ENTRYPOINT ["dotnet", "consumer_api.dll"]
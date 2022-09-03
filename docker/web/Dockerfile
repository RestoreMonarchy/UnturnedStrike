FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY /src/web/UnturnedStrikeWeb/UnturnedStrikeWeb.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish src/web/UnturnedStrikeWeb/UnturnedStrikeWeb.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "UnturnedStrikeWeb.dll"]
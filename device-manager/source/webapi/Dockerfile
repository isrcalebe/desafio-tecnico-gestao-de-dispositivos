# Build Stage

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ["source/common", "common/"]
COPY ["source/domain", "domain/"]
COPY ["source/application", "application/"]
COPY ["source/infrastructure", "infrastructure/"]
COPY ["source/webapi", "webapi/"]

WORKDIR /app/webapi
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Runtime Stage

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "DeviceManager.WebApi.dll"]

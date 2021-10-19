FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/merchandise-service/merchandise-service.csproj", "src/merchandise-service/"]
RUN dotnet restore "src/merchandise-service/merchandise-service.csproj"
COPY . .
WORKDIR "/src/src/merchandise-service"
RUN dotnet build "merchandise-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "merchandise-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "merchandise-service.dll"]
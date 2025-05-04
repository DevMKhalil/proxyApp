# Stage 1: build with SDK (Linux)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["proxyApp/proxyApp.csproj", "proxyApp/"]
RUN dotnet restore "proxyApp/proxyApp.csproj"
COPY . .
WORKDIR "/src/proxyApp"
RUN dotnet publish "proxyApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
# Render يحدد المنفذ عبر المتغيّر البيئي PORT
ENV ASPNETCORE_URLS=http://*:$PORT
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "proxyApp.dll"]

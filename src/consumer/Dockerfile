FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KC.Worker/KC.Worker.csproj", "KC.Worker/"]
RUN dotnet restore "KC.Worker/KC.Worker.csproj"
COPY . .
WORKDIR "/src/KC.Worker"
RUN dotnet build "KC.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KC.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KC.Worker.dll"]

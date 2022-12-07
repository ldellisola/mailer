FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["Mailer.csproj", "./"]
RUN dotnet restore "Mailer.csproj" 

COPY . .
WORKDIR "/src/"
RUN dotnet build "Mailer.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Mailer.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","Mailer.dll"]
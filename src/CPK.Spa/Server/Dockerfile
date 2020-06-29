#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CPK.Spa/Server/CPK.Spa.Server.csproj", "src/CPK.Spa/Server/"]
COPY ["src/CPK.Shared/CPK.Shared.csproj", "src/CPK.Shared/"]
COPY ["src/CPK.Spa/Client/CPK.Spa.Client.csproj", "src/CPK.Spa/Client/"]
RUN dotnet restore "src/CPK.Spa/Server/CPK.Spa.Server.csproj"
COPY . .
WORKDIR "/src/src/CPK.Spa/Server"
RUN dotnet build "CPK.Spa.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CPK.Spa.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CPK.Spa.Server.dll"]
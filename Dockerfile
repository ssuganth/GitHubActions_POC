#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DotnetCoreSampleApp/DotnetCoreSampleApp.csproj", "DotnetCoreSampleApp/"]
RUN dotnet restore "DotnetCoreSampleApp/DotnetCoreSampleApp.csproj"
COPY . .
WORKDIR "/src/DotnetCoreSampleApp"
RUN dotnet build "DotnetCoreSampleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotnetCoreSampleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotnetCoreSampleApp.dll"]
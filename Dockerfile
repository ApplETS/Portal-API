#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PortalApi/PortalApi.csproj", "PortalApi/"]
RUN dotnet restore "PortalApi/PortalApi.csproj"
COPY . .
WORKDIR "/src/PortalApi"
RUN dotnet build "PortalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PortalApi.csproj" -c Release -o /app/publish

RUN cp -R ./local/ /app/local

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortalApi.dll"]
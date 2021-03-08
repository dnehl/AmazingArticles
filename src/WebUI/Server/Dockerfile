#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/WebUI/Server/WebUI.Server.csproj", "src/WebUI/Server/"]
COPY ["src/WebUI/Client/WebUI.Client.csproj", "src/WebUI/Client/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/WebUI/Server/WebUI.Server.csproj"
COPY . .
WORKDIR "/src/src/WebUI/Server"
RUN dotnet build "WebUI.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.Server.dll"]
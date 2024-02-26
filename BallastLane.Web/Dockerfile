FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

LABEL description="BallastLane Api"

WORKDIR /app
EXPOSE 81

ENV ASPNETCORE_URLS=http://+:81

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["BallastLane.Web/BallastLane.Web.csproj", "BallastLane.Web/"]
RUN dotnet restore "BallastLane.Web/BallastLane.Web.csproj"
COPY . .
WORKDIR "/src/BallastLane.Web"
RUN dotnet build "BallastLane.Web.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BallastLane.Web.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BallastLane.Web.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Talksy.Api/Talksy.Api.csproj", "Talksy.Api/"]
RUN dotnet restore "Talksy.Api/Talksy.Api.csproj"
COPY . .
WORKDIR "/src/Talksy.Api"
RUN dotnet build "Talksy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Talksy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p Data
RUN chmod 777 Data
RUN echo "" > Data/app.db
ENTRYPOINT ["dotnet", "Talksy.Api.dll"]

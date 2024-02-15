FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# Create the uploads directory
#RUN mkdir -p /app/upload_images
#
## Set permissions for the uploads directory
#RUN chmod -R 755 /app/upload_images

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NpgSqlBackup.csproj", "./"]
RUN dotnet restore "NpgSqlBackup.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "NpgSqlBackup.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NpgSqlBackup.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NpgSqlBackup.dll"]

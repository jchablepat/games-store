# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["GameStore.Api/GameStore.Api.csproj", "GameStore.Api/"]
RUN dotnet restore "GameStore.Api/GameStore.Api.csproj"

# Copiar el resto del código y construir
COPY . .
WORKDIR "/src/GameStore.Api"
RUN dotnet build "GameStore.Api.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "GameStore.Api.csproj" -c Release -o /app/publish

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Crear directorio para la base de datos (importante para volúmenes)
RUN mkdir -p /data
# Asegurar permisos puede ser necesario dependiendo del usuario, pero en la imagen base de aspnet suele ser app user
# Por defecto .NET 8+ usa un usuario no root. 

ENTRYPOINT ["dotnet", "GameStore.Api.dll"]

# Etapa 1: Build

# Usa la imagen del SDK de .NET 7 para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Define el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia todo el código fuente al contenedor
COPY . .

# Restaura las dependencias del proyecto
RUN dotnet restore BG.sln

# Compila y publica la aplicación en modo Release, colocando los archivos en /out
RUN dotnet publish BG.sln -c Release -o /out



# Etapa 2: Runtime

# Usa la imagen más ligera del runtime de ASP.NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Define el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia solo los archivos compilados desde la etapa de compilación
COPY --from=build /out .

# Expone el puerto 5000 para que el contenedor escuche peticiones
EXPOSE 5024

# Comando de inicio de la aplicación
CMD ["dotnet", "BG.dll"]

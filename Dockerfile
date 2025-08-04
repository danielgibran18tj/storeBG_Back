# Etapa 1: Build

# Usa la imagen del SDK de .NET 7 para compilar el c�digo
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Define el directorio de trabajo dentro del contenedor
WORKDIR /app

COPY *.csproj ./
COPY *.sln ./
RUN dotnet restore

# Después copiar el resto del código
COPY . .

CMD ["dotnet", "watch", "run", "--project", "BG.csproj"]

# Compila y publica la aplicaci�n en modo Release, colocando los archivos en /out
#RUN dotnet publish BG.sln -c Release -o /out



# Etapa 2: Runtime

# Usa la imagen m�s ligera del runtime de ASP.NET 7
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Define el directorio de trabajo dentro del contenedor
#WORKDIR /app

# Copia solo los archivos compilados desde la etapa de compilaci�n
#COPY --from=build /out .

# Expone el puerto 5000 para que el contenedor escuche peticiones
#EXPOSE 5000

# Comando de inicio de la aplicaci�n
#CMD ["dotnet", "BG.dll"]




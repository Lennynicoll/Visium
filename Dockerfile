# ---------- Etapa de build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar la solucion y los archivos .csproj para restaurar con cache
COPY RegistroVisitantes.sln .
COPY RegistroVisitantes.Domain/RegistroVisitantes.Domain.csproj RegistroVisitantes.Domain/
COPY RegistroVisitantes.Application/RegistroVisitantes.Application.csproj RegistroVisitantes.Application/
COPY RegistroVisitantes.Infrastructure/RegistroVisitantes.Infrastructure.csproj RegistroVisitantes.Infrastructure/
COPY RegistroVisitantes.API/RegistroVisitantes.API.csproj RegistroVisitantes.API/

RUN dotnet restore RegistroVisitantes.sln

# Copiar el resto del codigo y publicar en Release
COPY . .
RUN dotnet publish RegistroVisitantes.API/RegistroVisitantes.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ---------- Etapa de runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

# URL que escucha la API
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "RegistroVisitantes.API.dll"]

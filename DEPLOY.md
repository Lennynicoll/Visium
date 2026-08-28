# 🌐 Visium — Despliegue del BACKEND en un VPS (Docker + SQL Server)

Este documento describe cómo el administrador del servidor (VPS) debe publicar
la **API backend** de Visium usando Docker, conectada a una base de datos
**SQL Server**.

> Nota: este documento cubre el **backend (API)**. El frontend React se publica
> aparte (p. ej. como sitio estático en cualquier hosting o en el mismo VPS).

---

## 1️⃣ Requisitos en el VPS

- **Docker** y **Docker Compose** instalados.
  ```bash
  docker --version
  docker compose version
  ```
- Un **SQL Server** accesible (puede ser:
  - un servidor SQL Server existente, o
  - un contenedor SQL Server, o
  - SQL Server corriendo en el VPS).

> ⚠️ `.NET 10` y el proyecto usan `Microsoft.EntityFrameworkCore.SqlServer 10.0.9`.

---

## 2️⃣ Variables de entorno (.env)

Copia el archivo de ejemplo y rellena los valores reales:

```bash
cp .env.example .env
nano .env
```

Variables mínimas que debes ajustar (las que terminan en `CAMBIAME`):

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | Cadena de conexión a SQL Server | `Server=localhost,1433;Database=RegistroVisitantesDb;User Id=sa;Password=TuClaveSegura;TrustServerCertificate=True;MultipleActiveResultSets=true` |
| `CORS__Origins` | Dominios permitidos (el del frontend), separados por `;` | `http://localhost:5173;https://visium.com` |

> ⚠️ **Nunca** subas el archivo `.env` a git (tiene credenciales). Está ignorado.

---

## 3️⃣ Opción A — Desplegar SOLO la API (SQL Server ya existente)

Si ya tienes un SQL Server al que conectarse, solo construye y corre el backend:

```bash
# 1. Construir la imagen
docker build -t visium-api .

# 2. Correr el contenedor (mapea el puerto 8080 del VPS al 8080 interno)
docker run -d \
  --name visium-api \
  --restart unless-stopped \
  -p 8080:8080 \
  --env-file .env \
  visium-api
```

**Aplicar las migraciones de la base de datos** (crea las tablas en SQL Server):

```bash
# Dentro del contenedor
docker exec -it visium-api dotnet ef database update \
  --project RegistroVisitantes.Infrastructure \
  --startup-project RegistroVisitantes.API
```

O si prefieres ejecutarlo localmente contra tu SQL Server:
```bash
dotnet ef database update --project RegistroVisitantes.Infrastructure --startup-project RegistroVisitantes.API
```

---

## 3️⃣ Opción B — Desplegar API + SQL Server con Docker Compose

Crea un `docker-compose.yml` (el proyecto ya incluye uno listo con `db` + `api`),
ajusta las variables y levanta todo:

```bash
# Levantar base de datos + API
docker compose --env-file .env up -d

# Ver logs
docker compose logs -f api
```

---

## 4️⃣ Verificar que la API corre

```bash
# Estado del contenedor
docker ps

# Probar el endpoint (Health/Swagger)
curl http://TU_IP_DEL_VPS:8080/swagger/index.html
curl http://TU_IP_DEL_VPS:8080/api/visitantes
```

Deberías ver el **Swagger** y las respuestas JSON de los endpoints.

> En `Production`, Swagger se deshabilita por defecto (solo activo en `Development`).
> Si el equipo quiere exponer Swagger en producción, hay que cambiarlo en `Program.cs`.

---

## 5️⃣ CORS

La API usa una política CORS llamada `AllowReact`. Para permitir que el
frontend la consuma, define `CORS__Origins` con el/los dominio(s) del frontend.

Ejemplo para producción:
```bash
# en .env
CORS__Origins=https://visium.com;https://www.visium.com
```

---

## 6️⃣ Problemas comunes

| Problema | Solución |
|----------|----------|
| La API no conecta a SQL Server | Verifica host/puerto (`localhost,1433`), credenciales y que SQL Server acepte conexiones TCP (puerto 1433 abierto). |
| Error `Falta la cadena de conexion` | No cargaste el `.env` o la variable no se llama exactamente `ConnectionStrings__DefaultConnection`. |
| CORS bloqueado desde el frontend | El dominio del frontend no está en `CORS__Origins`. |
| Puertos | Abre el puerto 8080 en el firewall del VPS. |

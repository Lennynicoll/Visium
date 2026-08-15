# Visium — Sistema de Registro de Visitantes

Sistema distribuido para controlar la **entrada y salida de visitantes** en edificios, empresas o residenciales. Registra visitantes, anfitriones, visitas, documentos de identidad (con control de vigencia) y mantiene un historial consultable.

## Tecnologías
- Backend: ASP.NET Core (C#) con Entity Framework Core y SQL Server
- Frontend: React + Vite
- Estructura en capas agrupadas en la solución `RegistroVisitantes.sln`

## Estructura por capas
| Proyecto | Capa | Responsabilidad |
|----------|------|-----------------|
| `RegistroVisitantes.Domain` | Domain | Entidades y reglas de negocio (POO) |
| `RegistroVisitantes.Application` | Application | DTOs, contratos y servicios |
| `RegistroVisitantes.Infrastructure` | Infrastructure | DbContext, repositorios y migraciones |
| `RegistroVisitantes.API` | API | Controladores REST y configuración |
| `patient-ui` | Cliente | Frontend React |

## Cómo ejecutar

### API (http://localhost:5297)
```bash
dotnet restore
dotnet ef database update --project RegistroVisitantes.Infrastructure --startup-project RegistroVisitantes.API
dotnet run --project RegistroVisitantes.API
```
Swagger disponible en http://localhost:5297/swagger

### Frontend (http://localhost:5173)
```bash
cd patient-ui
npm install
npm run dev
```

> La documentación completa (idea, alcance, requerimientos y conceptos POO) se entrega en la plataforma del aula.

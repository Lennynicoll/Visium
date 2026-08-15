# patient-ui - Frontend React del Sistema de Registro de Visitantes

Frontend SPA construido con React 19 + Vite. Consume la API REST del backend (`http://localhost:5297/api`) mediante Axios.

## Requisitos

- Node.js 18+
- Backend corriendo en `http://localhost:5297` (ver README.md raíz)

## Instalación y ejecución

```bash
npm install
npm run dev      # abre http://localhost:5173
npm run build    # compilación de producción (dist/)
npm run lint     # oxlint
```

## Módulos

- Dashboard
- Visitantes (CRUD + búsqueda por cédula)
- Anfitriones (vinculados a departamento y motivo de visita)
- Visitas (CRUD + control de entrada/salida por estado)
- Registro de Visitas (control en recepción/seguridad)
- Registro de Visitantes
- Oficinas, Motivos de Visita, Departamentos (catálogos)
- Documentos de identidad por visitante

## Estructura

```
src/
  api/           Servicios Axios por entidad
  components/    Layout, CrudPage (reutilizable)
  pages/         Pantallas por módulo
  App.jsx        Rutas (React Router)
  App.css        Estilos
```

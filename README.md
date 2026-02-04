# GameStore Project

Este repositorio contiene una solución completa para una tienda de videojuegos, dividida en dos módulos principales: Backend y Frontend.

## Estructura del Proyecto

El proyecto está organizado en dos directorios principales:

### 1. GameStore.Api (Backend)
Este módulo es una **API REST** construida con **ASP.NET Core Web API** utilizando **.NET 10**.
- **Tecnologías:** C#, Entity Framework Core, SQLite (para desarrollo).
- **Funcionalidad:** Proporciona endpoints para la gestión de juegos (CRUD) y listado de géneros.
- **Configuración:** Utiliza migraciones de EF Core para la base de datos.

### 2. GameStore.React (Frontend)
Este módulo es una aplicación de **Single Page Application (SPA)** construida con **React**.
- **Tecnologías:** React 18, Vite, TypeScript, Tailwind CSS.
- **Funcionalidad:** Interfaz de usuario para visualizar, crear, editar y eliminar juegos. Consume la API de `GameStore.Api`.
- **Características:**
  - Navegación con React Router.
  - Diseño responsivo con Tailwind CSS.
  - Gestión de variables de entorno (ver `.env.example`).

## Configuración y Ejecución

### Requisitos Previos
- .NET SDK 10.0 (o superior)
- Node.js (v18 o superior)

### Ejecutar Backend
1. Navega al directorio `GameStore.Api`.
2. Restaura las dependencias y aplica las migraciones (si es necesario):
   ```bash
   dotnet restore
   dotnet ef database update
   ```
3. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```
   La API estará disponible por defecto en `http://localhost:5089`.

### Ejecutar Frontend
1. Navega al directorio `GameStore.React`.
2. Instala las dependencias:
   ```bash
   npm install
   ```
3. Configura las variables de entorno:
   - Copia `.env.example` a `.env`.
   - Asegúrate de que `VITE_API_URL` apunte a tu backend (ej. `http://localhost:5089`).
4. Inicia el servidor de desarrollo:
   ```bash
   npm run dev
   ```
   La aplicación se abrirá en tu navegador (ej. `http://localhost:5173`).

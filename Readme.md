# Explorador BaaS — Rick & Morty (Monorepo)

Proyecto fullstack que consume la Rick & Morty API mediante un backend intermedio obligatorio.
- Backend: ASP.NET Core (.NET 8) + HttpClient tipado + EF Core + MySQL
- Frontend: Angular (standalone) consumiendo exclusivamente el backend propio vía proxy

## Estructura
- `/backend`  -> solución .NET (Script de mysql se encuentra aqui)
- `/frontend` -> Angular


---

# Requisitos
- .NET SDK 8
- Node 24 LTS + npm
- MySQL 8.x

---

# Base de datos (MySQL)

## Opción A: Script SQL (recomendado para entregable)
Ejecuta el script:
- `scripts/01_schema.sql`

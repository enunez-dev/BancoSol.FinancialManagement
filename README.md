# BancoSol Financial Management

API backend desarrollada como solución para una prueba técnica de gestión financiera personal. El sistema permite registrar ingresos, consultar historial de transacciones, obtener el tipo de cambio USD/BOB y generar reportes consolidados por período y moneda.

## Objetivo

La solución expone una API REST orientada a administrar ingresos personales en `BOB` y `USD`, separando claramente la lógica de negocio de la infraestructura y de la capa de transporte HTTP.

## Arquitectura

El proyecto fue construido aplicando:

- Arquitectura hexagonal
- Principios SOLID
- DDD pragmático
- Patrones `Repository` y `Adapter`

### Estructura de capas

- `Domain`: entidades, reglas de negocio, servicios de dominio, excepciones y value objects.
- `Application`: casos de uso, DTOs, contratos y coordinación de la lógica de aplicación.
- `Infrastructure`: persistencia con EF Core, repositorios, proveedor externo de tipo de cambio y configuraciones técnicas.
- `Web.API`: controladores, validaciones HTTP, middleware, Swagger y composición de dependencias.
- `Tests`: pruebas unitarias de dominio y casos de uso.

## Funcionalidades implementadas

- Registro de ingresos en `BOB` y `USD`
- Consulta paginada del historial de ingresos
- Consulta completa del historial mediante `fetchAll`
- Consulta de tipo de cambio `USD/BOB`
- Reporte de balance consolidado por rango de fechas y moneda objetivo
- Validación de requests con `FluentValidation`
- Manejo centralizado de errores
- Pruebas unitarias con cobertura
- Despliegue continuo con Azure DevOps y Azure App Service

## Stack tecnológico

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Supabase PostgreSQL
- FluentValidation
- Swagger / OpenAPI
- xUnit
- Moq
- Azure DevOps Pipelines
- Azure App Service

## Endpoints disponibles

### 1. Registrar ingreso

- Método: `POST`
- Ruta: `/api/incomes`

Ejemplo de request:

```json
{
  "amount": 1500.75,
  "description": "Pago de salario",
  "receivedOn": "2026-06-21",
  "source": "Empresa XYZ",
  "currency": "BOB"
}
```

### 2. Consultar historial de ingresos

- Método: `GET`
- Ruta: `/api/incomes`

Parámetros soportados:

- `page`
- `itemsPage`
- `startDate`
- `endDate`
- `fetchAll`

Ejemplo paginado:

```text
/api/incomes?page=1&itemsPage=10&startDate=2026-06-01&endDate=2026-06-30&fetchAll=0
```

Ejemplo para traer todo:

```text
/api/incomes?fetchAll=1
```

### 3. Consultar tipo de cambio USD/BOB

- Método: `GET`
- Ruta: `/api/exchange-rates/usd-bob`

### 4. Obtener balance consolidado

- Método: `GET`
- Ruta: `/api/reports/consolidated-balance`

Ejemplo:

```text
/api/reports/consolidated-balance?startDate=2026-06-01&endDate=2026-06-30&currency=BOB
```

## Reglas de negocio principales

- Solo se aceptan monedas `BOB` y `USD`
- El monto del ingreso debe ser mayor que cero
- El historial valida paginación y rango de fechas
- El balance consolidado convierte valores usando el tipo de cambio vigente `USD/BOB`
- La respuesta de errores de validación retorna `HTTP 400`

## Base de datos

La persistencia se implementó con `Entity Framework Core` sobre PostgreSQL. La solución está preparada para trabajar con Supabase PostgreSQL.

La entidad `Income` utiliza:

- identificador numérico autoincrementable
- persistencia mediante repositorio
- migraciones con EF Core

## Configuración

### Connection string

La aplicación espera una cadena de conexión en:

```json
"ConnectionStrings": {
  "DefaultConnection": "postgresql://USER:PASSWORD@HOST:5432/postgres"
}
```

### Proveedor externo de tipo de cambio

La URL completa del proveedor HexaRate debe configurarse mediante variable de entorno:

```text
HexaRate__UsdBobLatestUrl=https://hexarate.paikama.co/api/rates/USD/BOB/latest
```

En Azure App Service esta misma configuración debe agregarse en `Environment variables` o `Application settings` con ese mismo nombre.

## Ejecución local

### Requisitos

- .NET 10 SDK
- PostgreSQL disponible, o acceso a una base de datos Supabase

### Pasos

1. Restaurar dependencias:

```bash
dotnet restore
```

2. Configurar la cadena de conexión en `Web.API/appsettings.Development.json` o variables de entorno.

3. Configurar la variable de entorno de HexaRate.

PowerShell:

```powershell
$env:HexaRate__UsdBobLatestUrl="https://hexarate.paikama.co/api/rates/USD/BOB/latest"
```

4. Ejecutar la API:

```bash
dotnet run --project Web.API
```

5. Abrir Swagger:

```text
https://localhost:<puerto>/swagger
```

## Migraciones

La solución ejecuta `Database.MigrateAsync()` al iniciar la API, por lo que las migraciones pendientes se aplican automáticamente en el arranque.

Si se desea ejecutar manualmente:

```bash
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project Web.API/Web.API.csproj
```

## Validaciones y manejo de errores

- `FluentValidation` se usa para validar requests HTTP
- los errores de validación responden con estructura uniforme
- las reglas de dominio lanzan excepciones controladas
- existe middleware de manejo global de excepciones

## Pruebas unitarias

El proyecto incluye pruebas unitarias con:

- `xUnit`
- `Moq`
- `coverlet.collector`

Ejecutar tests:

```bash
dotnet test Tests/BancoSol.FinancialManagement.Tests.csproj
```

Ejecutar tests con cobertura:

```bash
dotnet test Tests/BancoSol.FinancialManagement.Tests.csproj --collect:"XPlat Code Coverage"
```

La cobertura generada puede transformarse a HTML con herramientas como `reportgenerator`.

## CI/CD

El proyecto incluye pipeline en `azure-pipelines.yml` con las siguientes etapas:

- restauración de dependencias
- compilación
- ejecución de pruebas
- publicación de cobertura
- publicación del artefacto
- despliegue a Azure App Service

## Consideración técnica relevante

Durante el despliegue se identificó que el proveedor externo HexaRate respondía inicialmente con `HTTP 403 Forbidden` debido a una protección de Cloudflare. Para resolverlo, el `HttpClient` fue configurado con headers HTTP estándar como:

- `Accept`
- `Accept-Language`
- `User-Agent`
- `Referrer`

Esto permitió consumir correctamente el tipo de cambio desde Azure App Service.

## Documentación

La documentación interactiva está disponible mediante Swagger en la ruta:

```text
/swagger
```

## Autor

- Edward Núñez

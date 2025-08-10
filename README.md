# Evoltis - Prueba Técnica Backend (.NET)

Tiene como objetivo evaluar tus conocimientos técnicos en el desarrollo de APIs RESTful utilizando .NET 6 y buenas prácticas de arquitectura y testing.

## Tecnologías utilizadas

- .NET 6
- Entity Framework Core (incluye Code First)
- AutoMapper
- xUnit (para testing)
- MySql como base de datos.
- Swagger (para documentación de endpoints).

## Estructura del Proyecto

El proyecto está basado en una arquitectura **monolítica por capas**. Las carpetas principales son:

- `Controllers`: Controladores con los endpoints de la API.
- `Domain`: Configuración del `DbContext` y archivos de configuración EF Core.
- `Models`: Modelos de dominio (Code First).
- `Repositories`: Implementación del patrón repositorio.
- `Services`: Contiene la lógica de negocio.
- `MappingServices`: Configuración de AutoMapper.
- `UnitTests`: Pruebas unitarias separadas por repositorio y servicios.
- `Program.cs` y `appsettings.json`: Archivos base para levantar la app.

## Consideraciones.
- Verificar la configuracion de program.cs para levantar el proyecto.
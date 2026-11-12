# 🛒 Comercio: Plataforma E-Commerce (.NET 9)

[![CI](https://github.com/damdavidprieto/Comercio/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damdavidprieto/Comercio/actions/workflows/dotnet.yml)
![Tech: .NET 9](https://img.shields.io/badge/Stack-.NET%209-512bd4?style=flat-square)
![Stack: Razor Pages](https://img.shields.io/badge/UI-Razor%20Pages-green?style=flat-square)
![Database: SQL Server](https://img.shields.io/badge/DB-SQL%20Server-red?style=flat-square)

> **Sobre este proyecto**
> Este repositorio constituye un ejemplo de plataforma e-commerce desarrollada con **.NET 9**. Se conserva como parte de un portfolio de ingeniería de software para demostrar competencias en el desarrollo de aplicaciones web server-side escalables y modelado de datos empresariales.

---

## 🎯 Arquitectura del Proyecto

El sistema está diseñado bajo principios de **arquitectura por capas**, optimizando el mantenimiento y la separación de preocupaciones:

- **Capa de Presentación (Web)**: Implementada con **Razor Pages**, proporcionando un modelo de programación basado en el servidor altamente productivo y con soporte nativo para SEO y rendimiento.
- **Capa de Infraestructura**: Gestiona el acceso a datos y las entidades de dominio. Utiliza modelos enriquecidos y un contexto de base de datos integrado.
- **Data Access**: Implementación robusta vía **Entity Framework Core 9**, optimizando la comunicación con SQL Server.

---

## 🛠️ Stack Tecnológico

*   **Runtime**: [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
*   **Web Environment**: **ASP.NET Core Razor Pages** para una gestión de vistas optimizada.
*   **Database ORM**: [Entity Framework Core 9.0](https://docs.microsoft.com/ef/core/).
*   **Asset Management**: Integración de assets estáticos y recursos multimedia escalables.
*   **Architectural Pattern**: Separación de lógica de negocio e infraestructura para facilitar la extensibilidad.

---

## 🚀 Despliegue Local

Para ejecutar este proyecto en tu entorno local:

1.  **Requisitos**: .NET 9 SDK y SQL Server configurado.
2.  **Configuración**: Ajustar `appsettings.json` con las credenciales de base de datos correspondientes.
3.  **Ejecución**:
    ```bash
    dotnet run
    ```
    La aplicación se iniciará en los puertos configurados (usualmente `https://localhost:5001`).

## 🧪 Tests

Los tests unitarios (xUnit, EF Core InMemory) se ejecutan en cada push con GitHub Actions:

```bash
dotnet test
```

---
*Este repositorio es un testimonio de ingeniería aplicada a soluciones comerciales modernas.*

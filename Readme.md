# Max Movie - REST API 🎬

API RESTful desarrollada en .NET 10, construida como evolución de una arquitectura MVC monolítica hacia un sistema cliente-servidor puro. Proporciona el backend robusto para una plataforma de gestión de películas, estructurada para ser consumida de forma segura por interfaces modernas (ej. React o JavaScript nativo).

## 🚀 Características Principales
* **Arquitectura REST:** Endpoints estructurados con operaciones CRUD completas, devolviendo respuestas HTTP estándar y transfiriendo datos mediante DTOs.
* **Seguridad (JWT):** Autenticación y Autorización basada en JSON Web Tokens con Identity Core, sin uso de cookies. Implementación de jerarquía de roles (`Admin` vs público) para proteger operaciones de mutación de datos.
* **Procesamiento Binario:** Soporte nativo para subida, validación y almacenamiento de imágenes físicas (`multipart/form-data`) en perfiles de usuario.
* **Documentación Interactiva:** Interfaz Swagger/OpenAPI configurada nativamente, con filtros de operación personalizados para testear endpoints protegidos con inyección de cabeceras de autorización.
* **CORS Configurado:** Políticas de orígenes cruzados habilitadas para facilitar la integración directa con clientes Frontend.

## 🛠️ Tecnologías Utilizadas
* C# / .NET 10
* Entity Framework Core & SQL Server
* ASP.NET Core Identity & JWT Bearer
* Swashbuckle (Swagger)

## ⚙️ Instalación y Ejecución
1. Clonar el repositorio.
2. Actualizar la cadena de conexión `MovieDbContext` en el archivo `appsettings.json` con tu instancia local de SQL Server.
3. Abrir la Consola del Administrador de Paquetes o la terminal y ejecutar la migración para crear la base de datos:
   ```bash
   Update-Database
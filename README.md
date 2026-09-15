# 🎬 Max Film - Sistema de Gestión

> 🚀 **Evolución del Proyecto (En progreso):** 
> Esta rama (`main`) contiene la versión estable del proyecto utilizando una arquitectura MVC tradicional. Actualmente, estoy trabajando en la refactorización completa de este ecosistema para convertirlo en una **Web API RESTful** (con JWT y ControllerBase) para desacoplar el backend del frontend. 
> 👉 **Podés ver el código de la API en progreso en la rama: [migracion-api] https://github.com/AngottiFederico/Max-movie/tree/migracion-api**

## 📝 Sobre el proyecto
Max Film es una aplicación web full-stack desarrollada como proyecto integrador para aplicar conceptos avanzados de programación en C# y el ecosistema .NET. El sistema permite la gestión completa de entidades, manejando la persistencia de datos mediante ORM y asegurando los accesos mediante autenticación y autorización de usuarios.

## 🛠️ Tecnologías y Arquitectura (Rama main)
* **Backend:** C#, .NET 10
* **Framework:** ASP.NET Core MVC
* **Acceso a Datos:** Entity Framework Core (Code-First)
* **Base de Datos:** Microsoft SQL Server
* **Seguridad:** ASP.NET Core Identity
* **Frontend:** HTML5, CSS3, Bootstrap, Razor Pages

## ⚙️ Características principales
* Arquitectura basada en el patrón Modelo-Vista-Controlador (MVC).
* Operaciones CRUD completas con validaciones tanto en el cliente como en el servidor.
* Sistema de registro, login y gestión de sesiones de usuario con Identity.
* Base de datos relacional modelada a través de Migraciones de Entity Framework.

## 🚀 Cómo ejecutar el proyecto localmente
1. Clonar este repositorio: `git clone https://github.com/AngottiFederico/Max-movie.git`
2. Abrir la solución `.sln` en Visual Studio.
3. Configurar la cadena de conexión (`DefaultConnection`) en el archivo `appsettings.json` apuntando a tu servidor SQL Server local.
4. Abrir la Consola del Administrador de Paquetes (Package Manager Console) y ejecutar el comando para crear la base de datos:
   `Update-Database`
5. Ejecutar la aplicación (F5 o botón de inicio en Visual Studio).

---
**Autor:** Federico Angotti López
*Estudiante de Analista Programador - Desarrollador .NET*
[Mi perfil de LinkedIn] https://www.linkedin.com/in/federico-angotti-lopez/
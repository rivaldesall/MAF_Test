# API REST de Gestión de Usuarios

Este proyecto implementa una API REST para las operaciones CRUD (Crear, Listar, Actualizar, Eliminar) de la entidad `Usuario`. Está construido sobre **.NET 8** y utiliza la **Arquitectura Limpia (Clean Architecture)**.

---

## Arquitectura del Proyecto (Clean Architecture)

El proyecto se basa en **Clean Architecture**, asegurando que el código de negocio sea independiente de los detalles de infraestructura.

| Proyecto | Capa | Rol Principal |
| :--- | :--- | :--- |
| `Domain` | Dominio | Definición de la entidad `Usuario`. |
| `Application` | Aplicación | Definición de interfaces de contratos (`IUnitOfWork`, `IUsuarioRepository`). |
| `Infrastructure` | Infraestructura | Implementación del acceso a datos usando Dapper y SQL Server. |
| `Api` | Presentación | Puntos de entrada (*Controllers*) y configuración del *host*. |

---

## Tecnologías y Librerías Clave

* **Framework:** .NET 8
* **Acceso a Datos:** **Dapper** (Micro-ORM de alto rendimiento).
* **Base de Datos:** SQL Server.
* **Contenedor:** Docker.
* **Testing:** **xUnit** y **Moq** (para pruebas unitarias).

---

## Configuración y Ejecución

### 1. Requisitos de la Base de Datos

**¡IMPORTANTE!** Para que el proyecto funcione, la base de datos `MAF_Usuarios` debe estar configurada.

1.  **Ejecute los Scripts de SQL:** Debe ejecutar el script SQL que contiene las sentencias `CREATE DATABASE`, `CREATE TABLE Usuario` y todos los `CREATE PROCEDURE` (Stored Procedures) requeridos para el CRUD.
2.  **Configuración de SQL Server:** Asegure que su instancia de SQL Server esté en **Modo de Autenticación Mixta** y que el protocolo **TCP/IP** esté habilitado con el puerto por defecto (1433).

### 2. Conectividad a SQL Server

La cadena de conexión debe usar **`host.docker.internal`** para que el contenedor pueda resolver la dirección de SQL Server en su máquina anfitriona (host).

**En `appsettings.json` o Variables de Entorno:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=host.docker.internal;Database=MAF_Usuarios;User Id=sa;Password=TuContrasenaSegura;TrustServerCertificate=True"
}
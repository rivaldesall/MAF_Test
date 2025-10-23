# API REST de Gestión de Usuarios

Este proyecto implementa una API REST para las operaciones CRUD (Crear, Listar, Actualizar, Eliminar) de la entidad `Usuario`. Está construido sobre **.NET 8** y utiliza la **Arquitectura Limpia (Clean Architecture)** para garantizar la separación de preocupaciones, la testeabilidad y la robustez transaccional.

---

## Arquitectura del Proyecto (Clean Architecture)

El proyecto sigue el patrón **Clean Architecture**, donde la dependencia siempre fluye de las capas externas (detalles) a las capas internas (políticas de negocio). Esto asegura que el código del núcleo sea independiente del framework y la base de datos.

| Proyecto | Capa | Rol Principal |
| :--- | :--- | :--- |
| `Domain` | 🟢 Dominio | Definición de la entidad `Usuario`. |
| `Application` | 🟡 Aplicación | Definición de interfaces de contratos (`IUnitOfWork`, `IUsuarioRepository`). |
| `Infrastructure` | 🟠 Infraestructura | Implementación del acceso a datos usando Dapper y SQL Server. |
| `API` | 🔴 Presentación | Puntos de entrada (*Controllers*) y configuración del *host*. |

---

## Tecnologías y Librerías Clave

| Librería | Categoría | Función Principal |
| :--- | :--- | :--- |
| **.NET 8** | Framework | Marco de desarrollo moderno y robusto. |
| **Dapper** | Micro-ORM | Mapeo rápido de datos de Stored Procedures a objetos C#. |
| **Microsoft.Data.SqlClient** | Acceso a Datos | Proveedor para la conexión física con SQL Server. |
| **xUnit** y **Moq** | Testing | Frameworks utilizados para las pruebas unitarias. |
| **Docker** | Contenedor | Entorno de despliegue para el aislamiento del servicio. |

---

## Configuración y Ejecución con Docker

### Conectividad a SQL Server

La cadena de conexión debe usar **`host.docker.internal`** para que el contenedor pueda resolver la dirección de SQL Server en tu máquina anfitriona. Se requiere el **Modo de Autenticación Mixta** habilitado en SQL Server.

**En `appsettings.json` o Variables de Entorno:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=host.docker.internal;Database=DB_Usuarios;User Id=sa;Password=TuContrasenaSegura;TrustServerCertificate=True"
}
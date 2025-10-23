CREATE DATABASE MAF_Usuarios;
GO

USE MAF_Usuarios
GO

-- 1. CREACIÓN DE LA TABLA
CREATE TABLE Usuario (
    IdUsuario INT NOT NULL IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Correo VARCHAR(150) UNIQUE NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    CONSTRAINT PK_Usuario PRIMARY KEY(IdUsuario)
);
GO

-- 2. SPs
CREATE PROCEDURE SP_Usuario_Crear
    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @Correo VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Usuario (Nombre, Apellido, Correo)
    VALUES (@Nombre, @Apellido, @Correo);
    
    SELECT IdUsuario, Nombre, Apellido, Correo, FechaCreacion, Activo 
    FROM Usuario 
    WHERE IdUsuario = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE SP_Usuario_ListarTodos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT IdUsuario, Nombre, Apellido, Correo, FechaCreacion, Activo
    FROM Usuario
    ORDER BY Apellido, Nombre;
END
GO

CREATE PROCEDURE SP_Usuario_Actualizar
    @IdUsuario INT,
    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @Correo VARCHAR(150),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Usuario
    SET 
        Nombre = @Nombre,
        Apellido = @Apellido,
        Correo = @Correo,
        Activo = @Activo
    WHERE IdUsuario = @IdUsuario;

    SELECT IdUsuario, Nombre, Apellido, Correo, FechaCreacion, Activo 
    FROM Usuario 
    WHERE IdUsuario = @IdUsuario;
END
GO

CREATE PROCEDURE SP_Usuario_Eliminar
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Usuario
    WHERE IdUsuario = @IdUsuario;
    
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO